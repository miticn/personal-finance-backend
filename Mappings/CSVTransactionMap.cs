using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Finance.Mappings
{
    public class CSVTransactionMap : ClassMap<Transaction.Models.Transaction>
    {
        CSVTransactionMap()
        {
            Map(m => m.Id).Name("id").TypeConverter<TrimConverter>();
            Map(m => m.BeneficiaryName).Name("beneficiary-name").TypeConverter<TrimConverter>();
            Map(m => m.Date).Name("date").TypeConverter<TrimConverter>().TypeConverter<DateTimeConverter>();
            Map(m => m.Direction).Name("direction");
            Map(m => m.Amount).Name("amount");
            Map(m => m.Description).Name("description").TypeConverter<TrimConverter>();
            Map(m => m.Currency).Name("currency").TypeConverter<TrimConverter>();
            Map(m => m.MCC).Name("mcc");
            Map(m => m.Kind).Name("kind");
        }
        
    }
    public class TrimConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text.Trim();
        }
    }
}
