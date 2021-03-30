using Sprache;

namespace TokyoChokoku.MarkinBox.Sketchbook
{
    public class FieldTextParser
    {
        private static readonly
        Parser <TextNode> TextGrammer = 
            from text in Parse.CharExcept ('@').AtLeastOnce ().Text ()
            select new TextNode (text);


        private static readonly
        Parser <TextNode> ErrorTextGrammer = 
            from consumerAt in Parse.Char('@')
            from text       in Parse.CharExcept ('@').AtLeastOnce ().Text ()
            select new TextNode (consumerAt + text);
        

        private static readonly
        Parser <LogoNode> LogoGrammer =
            from   header   in Parse.String ("@L[")
            from   id       in Parse.Digit.Repeat(2, 3).Text().Select (int.Parse)
            from   footer   in Parse.String ("]")
            select new LogoNode (id);


        private static readonly
        Parser <CalendarNode> CalendarGrammer =
            from   header   in Parse.String ("@C[")
            from    types   in (
                     Parse.String ("S"   ).Return (CalendarType.S    )
                .Or (Parse.String ("MM"  ).Return (CalendarType.MM   ))
                .Or (Parse.String ("M"   ).Return (CalendarType.M    ))
                .Or (Parse.String ("DD"  ).Return (CalendarType.DD   ))
                .Or (Parse.String ("D"   ).Return (CalendarType.D    ))
                .Or (Parse.String ("JJJ" ).Return (CalendarType.JJJ  ))
                .Or (Parse.String ("YYYY").Return (CalendarType.YYYY ))
                .Or (Parse.String ("YY"  ).Return (CalendarType.YY   ))
                .Or (Parse.String ("Y"   ).Return (CalendarType.Y    ))
                .AtLeastOnce ()
            )
            from   footer   in Parse.String ("]")
            select  new CalendarNode (types);



        private static readonly
        Parser <SerialNode> SerialGrammer   =
            from   header    in Parse.String ("@S[")
            from   current   in (
                from strCurrent in Parse.WhiteSpace.Or(Parse.Numeric).AtLeastOnce ().Text ()
                select int.Parse(strCurrent)
            )
            from   separator in Parse.Char ('-')
            from   serial    in  (
                from strSerial in Parse.Digit.AtLeastOnce ().Text ()
                select int.Parse (strSerial)
            )
            from   footer    in Parse.String ("]")
            select  new SerialNode (current, serial);




        private static readonly
        Parser <RootFieldTextNode> FieldTextGrammer =
            from    head  in (
                TextGrammer.Select ( (node) => (IFieldTextNode) node)
                .Or ( LogoGrammer      )
                .Or ( SerialGrammer    )
                .Or ( CalendarGrammer  )
                .Or ( ErrorTextGrammer )
                .Many ()
            )
            select new RootFieldTextNode (head);


        private static readonly
        Parser <RootFieldTextNode> FieldTextGrammerWithoutLogo =
            from    head  in (
                TextGrammer.Select ( (node) => (IFieldTextNode) node)
                .Or ( SerialGrammer    )
                .Or ( CalendarGrammer  )
                .Or ( ErrorTextGrammer )
                .Many ()
            )
            select new RootFieldTextNode (head);




        public static RootFieldTextNode ParseText (string source) {
            return FieldTextGrammer.Parse (source);
        }


        public static RootFieldTextNode ParseTextWithoutLogo (string source) {
            return FieldTextGrammerWithoutLogo.Parse (source);
        }



    }
}

