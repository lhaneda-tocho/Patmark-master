using System;
using System.Linq;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.Communication
{
    public class ReponseMBSerialCountersData : Response
    {
        public ReponseMBSerialCountersData (IRawResponse raw) : base(raw)
        {
        }

        public List<MBSerialCounterData> Value{
            get{
                var res = new List<MBSerialCounterData> ();
                for (var i = 0; i < Serial.Consts.NumOfSerial; i++) {
                    var dat = new MBSerialCounterData();
                    dat.SetBin(Raw.Data
                                .Skip(i * Serial.Consts.BytesOfSerialCounter)
                                .Take(Serial.Consts.BytesOfSerialCounter).ToArray()
                              );
                    res.Add(dat);
                }
                return res.OrderBy((counter) =>
                        {
                            return counter.SerialNo;
                }).ToList();
            }
        }
    }
}

