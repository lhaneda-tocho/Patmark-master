using System;

namespace TokyoChokoku.Patmark
{
    public static class LocalFilePathGeneratorPublisher
    {
        public static ILocalFilePathGenerator Instance{
            get; private set;
        }

        public static void Inject(ILocalFilePathGenerator impl){
            if (Instance == null)
            {
                Instance = impl;
            }
        }
    }
}
