using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ReponseMBSerialSettingsData : Response
    {
        public ReponseMBSerialSettingsData (IRawResponse raw) : base(raw)
        {
        }

        public List<MBSerialData> Value{
            get{
                var res = new List<MBSerialData> ();
                for (var i = 0; i < Serial.Consts.NumOfSerial; i++) {
                    var dat = new MBSerialData();
                    dat.SetBin(Raw.Data
                                .Skip(i * Serial.Consts.BytesOfSerialSetting)
                                .Take(Serial.Consts.BytesOfSerialSetting).ToArray()
                              );
                    res.Add(dat);
                }
                return res;
            }
        }
    }
}

