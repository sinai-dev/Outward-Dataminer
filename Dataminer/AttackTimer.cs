using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections;
using SideLoader;

namespace Dataminer
{
    public class AttackTimer : MonoBehaviour
    {
        public static AttackTimer Instance;

        public static bool TimerStarted = false;

        public static float ComboTimer = 0f;
        public static float LastComboTime = 0f;
        public static int ComboStep = 0;
        public static int ComboLength = 2;

        public static int LastAttackID = -1;
        public static DamageList LastDamage = new DamageList();

        internal void Awake()
        {
            Instance = this;
        }

        internal void Update()
        {
            if (TimerStarted)
            {
                ComboTimer += Time.deltaTime;
            }
        }

        [HarmonyPatch(typeof(Character), "ReceiveDamage")]
        public class Character_ReceiveDamage
        {
            [HarmonyPrefix]
            public static bool Prefix(Character __instance, float _damage, Vector3 _hitVec, bool _syncIfClient = true)
            {
                var self = __instance;

                SL.Log(string.Format("{0} | {1} received {2} damage", Math.Round(Time.time, 1), self.Name, Math.Round(_damage, 2)));

                return true;
            }
        }

        [HarmonyPatch(typeof(Character), "StartAttack")]
        public class Character_StartAttack
        {
            public static void Postfix(Character __instance, int _type, int _id)
            {
                var self = __instance;

                Instance.StartCoroutine(Instance.GetDamageCoroutine(self));

                if (ComboStep < ComboLength)
                {
                    if (!TimerStarted)
                    {
                        LastComboTime = ComboTimer;
                        ComboTimer = 0f;
                        TimerStarted = true;
                    }

                    ComboStep++;
                }
                else
                {
                    LastComboTime = ComboTimer;
                    ComboTimer = 0f;
                    ComboStep = 1;
                    TimerStarted = true;
                }
            }
        }

        private IEnumerator GetDamageCoroutine(Character self)
        {
            float start = Time.time;
            while (Time.time - start < 5f && LastAttackID == (int)typeof(Weapon).GetField("m_attackID", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self.CurrentWeapon))
            {
                yield return new WaitForSeconds(0.05f);
            }

            LastAttackID = (int)typeof(Weapon).GetField("m_attackID", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self.CurrentWeapon);
            LastDamage = LastAttackID == 0 ? self.CurrentWeapon.GetDamage(1) : self.CurrentWeapon.GetDamage(LastAttackID);
        }

        // =================================== GUI =============================================

        private Rect m_window = Rect.zero;

        internal void Start()
        {
            m_window = new Rect(5, 5, 275, 350);
        }

        internal void OnGUI()
        {
            m_window = GUI.Window(1001, m_window, TimerGUIFunc, "Attack Timer GUI");
        }

        private void TimerGUIFunc(int id)
        {
            GUI.DragWindow(new Rect(0, 0, m_window.width, 20));

            GUILayout.BeginArea(new Rect(5, 25, m_window.width - 10, m_window.height - 30));

            GUILayout.Label("Current Combo Length: " + AttackTimer.ComboLength);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("<") && AttackTimer.ComboLength > 1)
            {
                AttackTimer.ComboLength--;
            }
            if (GUILayout.Button(">"))
            {
                AttackTimer.ComboLength++;
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Current Combo Step: " + AttackTimer.ComboStep);
            if (GUILayout.Button("Reset"))
            {
                AttackTimer.ComboStep = 0;
                AttackTimer.ComboTimer = 0f;
                AttackTimer.TimerStarted = false;
                AttackTimer.LastAttackID = -1;
                AttackTimer.LastDamage = new DamageList();
            }
            GUILayout.Space(20);

            var time = TimeSpan.FromSeconds(AttackTimer.ComboTimer);
            GUILayout.Label("Timer: " + time.Seconds.ToString("00") + ":" + time.Milliseconds.ToString("000"));
            time = TimeSpan.FromSeconds(AttackTimer.LastComboTime);
            GUILayout.Label("Last Time: " + time.Seconds.ToString("00") + ":" + time.Milliseconds.ToString("000"));

            GUILayout.Space(20);

            GUILayout.Label("Last Attack ID: " + AttackTimer.LastAttackID);
            GUILayout.Label("Last damage: " + AttackTimer.LastDamage.ToString());

            GUILayout.EndArea();
        }
    }
}
