using System;

namespace TokyoChokoku.MarkinBox.Sketchbook.Validators
{
    public delegate
    void ModificationListener (ValidationResult result, object sender);


    public static class ModificationListenerExt {
        /// <summary>
        /// excludeに指定された1つのModificationListenerと一致するものを
        /// invocationListから除外します．
        /// この関数を適用した結果，invocationListが空になる場合は nullを返します．
        /// invocationListに null を指定された場合も null を返します.
        /// excludeに null を指定された場合は invocationList を返します．
        /// </summary>
        /// <param name="invocationList">Invocation list.</param>
        /// <param name="exclude">Exclude.</param>
        public static ModificationListener Exclude (
            this ModificationListener invocationList, ModificationListener exclude)
        {
            if (exclude == null)
                return invocationList;
            if (invocationList == null)
                return null;
            
            ModificationListener newList = null;

            newList += invocationList;
            newList -= exclude;

            return newList;
        }
    }
}

