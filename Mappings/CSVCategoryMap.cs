using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Finance.Models;

namespace Finance.Mappings
{
    public class CSVCategoryMap : ClassMap<Category>
    {
        CSVCategoryMap()
        {
            Map(m => m.Code).Name("code").TypeConverter<TrimConverter>();
            Map(m => m.Name).Name("name").TypeConverter<TrimConverter>();
            Map(m => m.ParentCode).Name("parent-code").TypeConverter<TrimConverter>();
        }
        
    }
}
