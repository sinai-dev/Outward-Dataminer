using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace Dataminer
{
    /// <summary>
    /// Attribute used to mark a type that needs to be serialized by the Serializer.
    /// Usage is to just put [DM_Serialized] on a base class. Derived classes will inherit it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DM_Serialized : Attribute { }

    /// <summary>
    /// Dataminer's serializer. Handles Xml serialization for Dataminer's custom types.
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// Dataminer.dll AppDomain reference.
        /// </summary>
        public static Assembly DM_Assembly
        {
            get
            {
                if (m_DMAssembly == null)
                {
                    // We should be able to get it this way
                    m_DMAssembly = Assembly.GetExecutingAssembly();

                    // If for some reason it doesnt work (perhaps called by another mod from outside Dataminer.dll before Dataminer initializes?)
                    if (!m_DMAssembly.FullName.Contains("Dataminer"))
                    {
                        m_DMAssembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName.Contains("Dataminer"));
                    }
                }

                return m_DMAssembly;
            }
        }

        private static Assembly m_DMAssembly; 
        
        /// <summary>
        /// The Assembly-Csharp.dll AppDomain reference.
        /// </summary>
        public static Assembly Game_Assembly
        {
            get
            {
                if (m_gameAssembly == null)
                {
                    m_gameAssembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == m_gameAssemblyFullName);
                }

                return m_gameAssembly;
            }
        }

        private const string m_gameAssemblyFullName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        private static Assembly m_gameAssembly;

        /// <summary>
        /// List of DM_Type classes (types marked as DM_Serialized).
        /// </summary>
        public static Type[] DMTypes
        {
            get
            {
                if (m_dmTypes == null || m_dmTypes.Length < 1)
                {
                    var list = new List<Type>();
                    foreach (var type in DM_Assembly.GetTypes())
                    {
                        // check if marked as DM_Serialized
                        if (type.GetCustomAttributes(typeof(DM_Serialized), true).Length > 0)
                        {
                            list.Add(type);
                        }
                    }

                    // add other types
                    list.AddRange(new Type[]
                    {
                        typeof(WeaponStats.AttackData),
                    });

                    m_dmTypes = list.ToArray();
                }

                return m_dmTypes;
            }
        }

        private static Type[] m_dmTypes;

        public static Type GetBestDMType(Type type)
        {
            if (GetDMType(type, false) is Type slType)
            {
                return slType;
            }
            else
            {
                return GetBestDMType(type.BaseType);
            }
        }

        /// <summary>
        /// Pass a Game Class type (eg, Item) and get the corresponding Dataminer class (eg, DM_Item).
        /// </summary>
        /// <param name="_gameType">Eg, typeof(Item)</param>
        /// <param name="logging">If you want to log debug messages.</param>
        public static Type GetDMType(Type _gameType, bool logging = true)
        {
            var name = $"Dataminer.DM_{_gameType.Name}";

            Type t = null;
            try
            {
                t = DM_Assembly.GetType(name);
                if (t == null) throw new Exception("Null");
            }
            catch (Exception e)
            {
                if (logging)
                {
                    Debug.LogWarning($"Could not get DM_Assembly Type '{name}'");
                    Debug.LogWarning(e.Message);
                    Debug.LogWarning(e.StackTrace);
                }
            }

            return t;
        }

        /// <summary>
        /// Pass a Dataminer class type (eg, DM_Item) and get the corresponding Game class (eg, Item).
        /// </summary>
        /// <param name="_dmType">Eg, typeof(DM_Item)</param>
        /// <param name="logging">If you want to log debug messages.</param>
        public static Type GetGameType(Type _dmType, bool logging = true)
        {
            var name = _dmType.Name.Substring(3, _dmType.Name.Length - 3);

            Type t = null;
            try
            {
                t = Game_Assembly.GetType(name);
                if (t == null) throw new Exception("Null");
            }
            catch (Exception e)
            {
                if (logging)
                {
                    Debug.LogWarning($"Could not get Game_Assembly Type '{name}'");
                    Debug.LogWarning(e.Message);
                    Debug.LogWarning(e.StackTrace);
                }
            }

            return t;
        }

        /// <summary>
        /// Save an DM_Type object to xml.
        /// </summary>
        public static void SaveToXml(string dir, string saveName, object obj)
        {
            if (!string.IsNullOrEmpty(dir))
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                dir += "/";
            }

            saveName = ReplaceInvalidChars(saveName);

            string path = dir + saveName + ".xml";
            if (File.Exists(path))
            {
                //Debug.LogWarning("SaveToXml: A file already exists at " + path + "! Deleting...");
                File.Delete(path);
            }

            XmlSerializer xml = new XmlSerializer(obj.GetType(), DMTypes);
            FileStream file = File.Create(path);
            xml.Serialize(file, obj);
            file.Close();
        }

        /// <summary>
        /// Load an DM_Type object from XML.
        /// </summary>
        public static object LoadFromXml(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("LoadFromXml :: Trying to load an XML but path doesnt exist: " + path);
                return null;
            }

            // First we have to find out what kind of Type this xml was serialized as.
            string typeName = "";
            using (XmlReader reader = XmlReader.Create(path))
            {
                while (reader.Read()) // just get the first element (root) then break.
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        // the real type might be saved as an attribute
                        if (!string.IsNullOrEmpty(reader.GetAttribute("type")))
                        {
                            typeName = reader.GetAttribute("type");
                        }
                        else
                        {
                            typeName = reader.Name;
                        }
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(typeName) && DM_Assembly.GetType($"Dataminer.{typeName}") is Type type)
            {
                XmlSerializer xml = new XmlSerializer(type, DMTypes);
                FileStream file = File.OpenRead(path);
                var obj = xml.Deserialize(file);
                file.Close();
                return obj;
            }
            else
            {
                Debug.Log("LoadFromXml Error, could not serialize the Type of document! typeName: " + typeName);
                return null;
            }
        }

        /// <summary>Remove invalid filename characters from a string</summary>
        public static string ReplaceInvalidChars(string s)
        {
            return string.Join("_", s.Split(Path.GetInvalidFileNameChars()));
        }

        /// <summary>
        /// Helpers for the folder paths Dataminer uses.
        /// </summary>
        public class Folders
        {
            /// <summary>The main Dataminer folder, 'Outward\Dataminer\'</summary>
            public const string Main = "Dataminer";

            /// <summary>The folder for merchants, 'Outward\Dataminer\Merchants\'</summary>
            public const string Merchants = Main + @"\Merchants";

            /// <summary>The folder for enemies, 'Outward\Dataminer\Enemies\'</summary>
            public const string Enemies = Main + @"\Enemies";

            /// <summary>The folder for scenes, 'Outward\Dataminer\Scenes\'</summary>
            public const string Scenes = Main + @"\Scenes";

            /// <summary>The folder for lists, 'Outward\Dataminer\Lists\'</summary>
            public const string Lists = Main + @"\Lists";

            /// <summary>The folder for prefabs, 'Outward\Dataminer\Prefabs\'</summary>
            public const string Prefabs = Main + @"\Prefabs";

            /// <summary>The folder for item prefabs, 'Outward\Dataminer\Prefabs\Items'</summary>
            public const string Items = Prefabs + @"\Items";

            /// <summary>The folder for status/imbue effect prefabs, 'Outward\Dataminer\Effects\'</summary>
            public const string Effects = Prefabs + @"\Effects";

            /// <summary>The folder for recipes, 'Outward\Dataminer\Prefabs\Recipes'</summary>
            public const string Recipes = Prefabs + @"\Recipes";

            /// <summary>The folder for drop tables prefabs, 'Outward\Dataminer\Prefabs\DropTables\'</summary>
            public const string DropTables = Prefabs + @"\DropTables";

            public static bool MakeFolders()
            {
                bool madeFolder = false;
                foreach (FieldInfo fi in typeof(Folders).GetFields(At.FLAGS))
                {
                    string path = (string)fi.GetValue(null);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        madeFolder = true;
                    }
                }
                return madeFolder;
            }
        }
    }
}
