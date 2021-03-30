using System;
namespace TokyoChokoku.Patmark
{
    public static class CommonStrings
    {
        internal static ICommonStrings Instance { get; private set; }

        public static void Inject(ICommonStrings impl){
            if(Instance == null){
                Instance = impl;
            }
        }
    }
}
