/*
Copyright (c) 2014~2016, Justin Bengtson
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

  Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

  Redistributions in binary form must reproduce the above copyright notice, this
  list of conditions and the following disclaimer in the documentation and/or
  other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using Random = UnityEngine.Random;

namespace Renamer
{
    public class Randomizer
    {
        public static void RerollKerbal(ProtoCrewMember kerbal, Culture[] cultures)
        {
            RerollKerbal(kerbal, RenamerCustomParams.BellCurveEnabled, RenamerCustomParams.DontInsultMeEnabled, RenamerCustomParams.GetBadassPercentage,
                         RenamerCustomParams.GetFemalePercentage, cultures, RenamerCustomParams.PreserveOriginalTraitsEnabled);
        }

        public static void RerollKerbal(ProtoCrewMember kerbal, bool useBellCurveMethod, bool dontInsultMe, float badassPercent, float femalePercent, Culture[] cultures, bool keepRoles)
        {
            LogUtils.Log("Rerolling kerbal ", kerbal.name);
            UnityEngine.Random.InitState(DateTime.Now.Millisecond * kerbal.name.GetHashCode());

            if (kerbal.type == ProtoCrewMember.KerbalType.Crew || kerbal.type == ProtoCrewMember.KerbalType.Applicant)
            {
                if (useBellCurveMethod || dontInsultMe)
                {
                    kerbal.stupidity = rollStupidity(useBellCurveMethod, dontInsultMe);
                    kerbal.courage = rollCourage(useBellCurveMethod);
                }

                kerbal.isBadass = UnityEngine.Random.Range(0.0f, 1.0f) < badassPercent;

                float traitRoll = UnityEngine.Random.Range(0.0f, 1.0f);
                if (keepRoles)
                {
                    KerbalRoster.SetExperienceTrait(kerbal, kerbal.trait);
                }
                else
                {
                    if (traitRoll < 0.33f)
                    {
                        KerbalRoster.SetExperienceTrait(kerbal, "Pilot");
                    }
                    else if (traitRoll < 0.66f)
                    {
                        KerbalRoster.SetExperienceTrait(kerbal, "Engineer");
                    }
                    else
                    {
                        KerbalRoster.SetExperienceTrait(kerbal, "Scientist");
                    }
                }

                if (UnityEngine.Random.Range(0.0f, 1.0f) <= femalePercent)
                {
                    kerbal.gender = ProtoCrewMember.Gender.Female;
                }
                else
                {
                    kerbal.gender = ProtoCrewMember.Gender.Male;
                }
            }

            RenameProtoCrewMember(kerbal, cultures);
        }

        /// <summary>
        /// Uses the roulette method to pick a culture from a profile.
        /// </summary>
        /// <param name="cultures"></param>
        /// <returns></returns>
        public static Culture SelectRandomCulture(Culture[] cultures)
        {
            Dictionary<string, double> wheel = KerbalRenamer.Instance.cultureWheel;
            double roll = (double) Random.Range(0f, 1f);

            string lastseen = "";
            foreach (KeyValuePair<string, double> kvp in wheel)
            {
                lastseen = kvp.Key;
                if (roll <= kvp.Value)
                {
                    break;
                }
                else
                {
                    roll -= kvp.Value;
                }
            }

            foreach (Culture culture in cultures)
            {
                if (culture.cultureName == lastseen) return culture;
            }
            
            // something went wrong.
            LogUtils.Log("Something went wrong in culture selection");
            return cultures[UnityEngine.Random.Range(0, cultures.Length)];
        }

        public static void GenerateRandomName(ProtoCrewMember.Gender gender, ref string culture, ref string name, Culture[] cultures)
        {
            Culture parent = SelectRandomCulture(cultures);
            name = parent.GenerateRandomName(gender);
            culture = parent.cultureName;
        }

        public static void RenameProtoCrewMember(ProtoCrewMember crewMember, Culture[] cultures)
        {
            string newname = "";
            string newculture = "";
            GenerateRandomName(crewMember.gender, ref newculture, ref newname, cultures);

            if (newculture.Length > 0)
            {
                crewMember.flightLog.AddEntryUnique(new FlightLog.Entry(0, KerbalRenamer.Instance.cultureDescriptor,
                    newculture));
            }
            
            LogUtils.Log("Renaming to ", newname);
            if (newname.Length > 0)
            {
                crewMember.ChangeName(newname);
            }
        }

        public static float rollCourage(bool useBellCurveMethod)
        {
            if (useBellCurveMethod)
            {
                float retval = 0;
                for (int i = 0; i < 5; i++)
                {
                    retval += UnityEngine.Random.Range(0f, 0.2f);
                }
                return retval;
            }
            else
            {
                return UnityEngine.Random.Range(0f, 1f);
            }
        }

        public static float rollStupidity(bool useBellCurveMethod, bool dontInsultMe)
        {
            if (useBellCurveMethod)
            {
                float retval = 0;
                int end = dontInsultMe ? 3 : 5;
                for (int i = 0; i < end; i++)
                {
                    retval += UnityEngine.Random.Range(0f, 0.2f);
                }
                return retval;
            }
            else
            {
                return UnityEngine.Random.Range(0.0f, 1.0f);
            }
        }
    }
}