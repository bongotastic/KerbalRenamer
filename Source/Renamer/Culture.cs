using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

namespace Renamer
{
    public class Culture
    {
        /// <summary>
        /// Whether a culture places last name ahead of first.
        /// </summary>
        public bool reversePattern = false;
        
        /// <summary>
        /// Whether a culture uses a different family name for female names
        /// </summary>
        public bool femaleSurnamesExist = false;
        
        /// <summary>
        /// Identifier of the culture as defined in config files
        /// </summary>
        
        public string cultureName = "";
        public string[] fnames1 = { };
        public string[] fnames2 = { };
        public string[] fnames3 = { };
        public string[] mnames1 = { };
        public string[] mnames2 = { };
        public string[] mnames3 = { };
        public string[] lnames1 = { };
        public string[] lnames2 = { };
        public string[] lnames3 = { };
        public string[] flnames1 = { };
        public string[] flnames2 = { };
        public string[] flnames3 = { };

        public Culture(ConfigNode node)
        {
            string[] vals;
            cultureName = node.HasValue("name") ? node.GetValue("name") : "";
            reversePattern = node.HasValue("pattern") && (node.GetValue("pattern") == "LF");

            foreach (ConfigNode childNode in node.nodes)
            {
                vals = childNode.GetValues("key");
                if (vals.Length > 0)
                {
                    switch (childNode.name)
                    {
                        case "FFIRSTNAME1":
                            fnames1 = vals;
                            break;
                        case "FFIRSTNAME2":
                            fnames2 = vals;
                            break;
                        case "FFIRSTNAME3":
                            fnames3 = vals;
                            break;
                        case "MFIRSTNAME1":
                            mnames1 = vals;
                            break;
                        case "MFIRSTNAME2":
                            mnames2 = vals;
                            break;
                        case "MFIRSTNAME3":
                            mnames3 = vals;
                            break;
                        case "LASTNAME1":
                            lnames1 = vals;
                            break;
                        case "LASTNAME2":
                            lnames2 = vals;
                            break;
                        case "LASTNAME3":
                            lnames3 = vals;
                            break;
                        case "FLASTNAME1":
                            flnames1 = vals;
                            break;
                        case "FLASTNAME2":
                            flnames2 = vals;
                            break;
                        case "FLASTNAME3":
                            flnames3 = vals;
                            break;
                        default:
                            break;
                    }

                    if (childNode.name.StartsWith("FLASTNAME"))
                    {
                        femaleSurnamesExist = true;
                    }
                }
            }
        }

        /// <summary>
        /// Create a full name for a given gender within this culture. For the API, use the calls in the
        /// kerbalRenamer class. 
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public string GenerateRandomName(ProtoCrewMember.Gender gender)
        {
            string firstName = GenerateRandomFirstName(gender);
            string lastName = GenerateRandomLastName(gender);

            if (!reversePattern)
            {
                if (firstName!= "" & lastName != "")
                {
                    return firstName + " " + lastName;
                }
                else
                {
                    return firstName + lastName;
                }
            }
            else
            {
                if (firstName!= "" & lastName != "")
                {
                    return lastName + " " + firstName;
                }
                else
                {
                    return lastName + firstName;
                }
            }
        }

        public string GenerateRandomFirstName(ProtoCrewMember.Gender gender)
        {
            string firstName = "";
            
            if (gender == ProtoCrewMember.Gender.Female)
            {
                if (fnames1.Length > 0)
                {
                    firstName += fnames1[UnityEngine.Random.Range(0, fnames1.Length)];
                }
                if (fnames2.Length > 0)
                {
                    firstName += fnames2[UnityEngine.Random.Range(0, fnames2.Length)];
                }
                if (fnames3.Length > 0)
                {
                    firstName += fnames3[UnityEngine.Random.Range(0, fnames3.Length)];
                }
            }
            else
            {
                if (mnames1.Length > 0)
                {
                    firstName += mnames1[UnityEngine.Random.Range(0, mnames1.Length)];
                }
                if (mnames2.Length > 0)
                {
                    firstName += mnames2[UnityEngine.Random.Range(0, mnames2.Length)];
                }
                if (mnames3.Length > 0)
                {
                    firstName += mnames3[UnityEngine.Random.Range(0, mnames3.Length)];
                }
            }
            return firstName;
        }

        public string GenerateRandomLastName(ProtoCrewMember.Gender gender)
        {
            string lastName = "";
            if (femaleSurnamesExist && gender == ProtoCrewMember.Gender.Female)
            {
                if (flnames1.Length > 0)
                {
                    lastName += flnames1[UnityEngine.Random.Range(0, flnames1.Length)];
                }
                if (flnames2.Length > 0)
                {
                    lastName += flnames2[UnityEngine.Random.Range(0, flnames2.Length)];
                }
                if (flnames3.Length > 0)
                {
                    lastName += flnames3[UnityEngine.Random.Range(0, flnames3.Length)];
                }
            }
            else
            {
                if (lnames1.Length > 0)
                {
                    lastName += lnames1[UnityEngine.Random.Range(0, lnames1.Length)];
                }
                if (lnames2.Length > 0)
                {
                    lastName += lnames2[UnityEngine.Random.Range(0, lnames2.Length)];
                }
                if (lnames3.Length > 0)
                {
                    lastName += lnames3[UnityEngine.Random.Range(0, lnames3.Length)];
                }
            }

            return lastName;
        }
    }
}
