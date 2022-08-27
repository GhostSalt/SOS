using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SOS
{
    class Data
    {
        public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ?????!";
        public static readonly List<int[]> FlightPaths = new List<int[]>()
        {
            new int[]
            {
                0, 1, 2, 3,
                7, 11, 15, 14,
                13, 12, 8, 9,
                10, 6, 5, 4
            },
            new int[]
            {
                6, 7, 3, 2,
                1, 0, 4, 5,
                9, 8, 12, 13,
                14, 15, 11, 10
            },
            new int[]
            {
                9, 5, 1, 0,
                4, 8, 12, 13,
                14, 15, 11, 7,
                3, 2, 6, 10
            },
            new int[]
            {
                15, 11, 7, 6,
                3, 2, 1, 0,
                5, 4, 8, 12,
                13, 9, 10, 14
            },

            new int[]
            {
                1, 0, 4, 8,
                12, 9, 6, 3,
                7, 11, 15, 14,
                13, 10, 5, 2
            },
            new int[]
            {
                7, 11, 15, 10,
                14, 13, 12, 8,
                4, 9, 5, 0,
                1, 2, 6, 3
            },
            new int[]
            {
                10, 13, 14, 15,
                11, 7, 3, 2,
                1, 0, 4, 8,
                12, 9, 5, 6
            },
            new int[]
            {
               12, 9, 8, 4,
               0, 1, 5, 6,
               2, 3, 7, 11,
               15, 10, 14, 13
            },

            new int[]
            {
               2, 3, 6, 7,
               10, 11, 15, 14,
               9, 13, 12, 8,
               5, 4, 0, 1
            },
            new int[]
            {
                4, 8, 12, 13,
                9, 10, 15, 14,
                11, 7, 3, 2,
                1, 6, 5, 0
            },
            new int[]
            {
                11, 15, 14, 10,
                9, 13, 12, 8,
                4, 0, 1, 2,
                5, 6, 3, 7
            },
            new int[]
            {
                13, 14, 15, 10,
                11, 7, 6, 3,
                2, 1, 0, 5,
                4, 8, 9, 12
            },

            new int[]
            {
                3, 6, 7, 11,
                15, 14, 10, 13,
                12, 8, 9, 5,
                4, 0, 1, 2
            },
            new int[]
            {
                5, 0, 1, 2,
                3, 6, 7, 11,
                15, 14, 9, 10,
                13, 12, 8, 4
            },
            new int[]
            {
                15, 11, 7, 3,
                2, 1, 0, 4,
                8, 13, 12, 9,
                6, 5, 10, 14
            },
            new int[]
            {
                14, 15, 11, 6,
                2, 3, 7, 10,
                9, 4, 0, 1,
                5, 8, 12, 13
            },

            new int[]
            {
                0, 5, 10, 6,
                1, 2, 3, 7,
                11, 15, 14, 13,
                12, 8, 9, 4
            },
            new int[]
            {
                6, 1, 2, 3,
                7, 10, 15, 11,
                14, 13, 12, 9,
                8, 4, 0, 5
            },
            new int[]
            {
                9, 10, 7, 11,
                15, 14, 13, 12,
                8, 4, 5, 0,
                1, 2, 3, 6
            },
            new int[]
            {
                15, 11, 7, 3,
                2, 6, 5, 1,
                0, 4, 8, 12,
                13, 9, 10, 14
            },

            new int[]
            {
                1, 6, 10, 13,
                8, 12, 9, 14,
                15, 11, 7, 3,
                2, 5, 0, 4
            },
            new int[]
            {
                7, 11, 15, 14,
                10, 6, 5, 9,
                13, 12, 8,  4,
                0, 1, 2, 3
            },
            new int[]
            {
                10, 14, 13, 12,
                8, 4, 0, 1,
                2, 5, 9, 6,
                3, 7, 11, 15
            },
            new int[]
            {
                12, 13, 14, 15,
                11, 10, 9, 5,
                6, 7, 3, 2,
                1, 0, 4, 8
            },

            new int[]
            {
                2, 3, 7, 11,
                15, 14, 13, 12,
                8, 4, 0, 1,
                6, 9, 10, 5
            },
            new int[]
            {
                4, 0, 1, 2,
                6, 3, 7, 11,
                15, 10, 5, 9,
                14, 13, 12, 8
            },
            new int[]
            {
                11, 7, 2, 3,
                6, 9, 5, 1,
                0, 4, 8, 12,
                13, 14, 15, 10
            },
            new int[]
            {
                13, 14, 10, 15,
                11, 7, 3, 6,
                2, 1, 5, 0,
                4, 8, 12, 9
            },

            new int[]
            {
                3, 7, 11, 15,
                10, 5, 0, 4,
                8, 12, 13, 14,
                9, 6, 1, 2
            },
            new int[]
            {
                5, 0, 1, 4,
                8, 13, 12, 9,
                10, 15, 14, 11,
                7, 2, 3, 6
            },
            new int[]
            {
                8, 12, 9, 13,
                14, 15, 11, 10,
                6, 7, 3, 2,
                1, 5, 0, 4
            },
            new int[]
            {
                14, 13, 8, 12,
                9, 5, 4, 0,
                1, 2, 3, 7,
                6, 10, 15, 11
            }
        };
        public static readonly string[] Messages =
        {
            "AUBERGINE",
            "BERYLLIUM",
            "CHIHUAHUA",
            "DIAMANTES",
            "EXUBERANT",
            "FLUMMOXED",
            "GYROSCOPE",
            "HEARKENED",
            "INOCULATE",
            "JUVENILIA",
            "KEROSENES",
            "LAUGHABLY",
            ""
        };
        public static List<bool> DecToBin(int input, int digits)
        {
            if (Mathf.Pow(2, digits) <= input)
                throw new System.Exception("NumToBin: The given input cannot be expressed with the number of digits specified!");
            else if (input < 0)
                throw new System.Exception("NumToBin: The given input is negative!");
            List<bool> output = new List<bool>();
            for (int i = 0; i < digits; i++)
            {
                output.Add(input % 2 == 1);
                input /= 2;
            }
            output.Reverse();
            return output;
        }
        public static int BinToDec(bool[] input)
        {
            int num = 0;
            for (int i = 0; i < input.Count(); i++)
                num += input[i] ? Mathf.FloorToInt(Mathf.Pow(2, input.Count() - i - 1)) : 0;
            return num;
        }
    }
}