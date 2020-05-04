**Describe the bug**
When reading some data that starts with a line-break, I'd expect it to receive a blank line for the first record and the actual data for the second record. If `IgnoreBlankLines = true`, you should only get one record because the first, empty record is ignored. This works fine for NL (`\n`) and CRNL (`\r\n`) but not CR (`\r`). With only CR, it skips a character.

**To Reproduce**
```
var s = new StringBuilder();
s.Append("\r1,2\r");
using (var reader = new StringReader(s.ToString()))
using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
{
    parser.Configuration.Delimiter = ",";

    var row = parser.Read(); // results in ["","2"] but should be ["1","2"]
}
```

It's the same for multiple rows.
```
var s = new StringBuilder();
s.Append("1,2\r");
s.Append("\r3,4\r");
using (var reader = new StringReader(s.ToString()))
using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
{
    parser.Configuration.Delimiter = ",";

    var row = parser.Read(); // results in ["1","2"] which is fine
    row = parser.Read(); // results in ["","4"] but should be ["3","4"]
}
```

Same for a single character (but this one is a bit special, I'll get to that)
```
var s = new StringBuilder();
s.Append("\r1\r");
using (var reader = new StringReader(s.ToString()))
using (var parser = new CsvParser(reader, CultureInfo.InvariantCulture))
{
    var row = parser.Read(); // results in null but should be ["1"]
}
```

**Expected behavior**
Mostly written in the tests but one addition: If you replace all the `\r` in the test samples with `\r\n` or `\n`, all of them will work.

**Issue**
The issue is in [CsvParser.tt](https://github.com/JoshClose/CsvHelper/blob/master/src/CsvHelper/CsvParser.tt#L230-L231). The offset returned by `ReadLineEnding` is ignored instead of used to correct the buffer position.
The PR to fix this is COMING SOON.