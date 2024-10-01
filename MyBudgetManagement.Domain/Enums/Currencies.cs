using System.ComponentModel;

namespace MyBudgetManagement.Domain.Enums;

public enum Currencies
{
    [Description("USD - United States Dollar")] USD = 840,
    [Description("EUR - Euro")] EUR = 978,
    [Description("JPY - Japanese Yen")] JPY = 392,
    [Description("GBP - British Pound")] GBP = 826,
    [Description("CNY - Chinese Yuan")] CNY = 156,
    [Description("AUD - Australian Dollar")] AUD = 36,
    [Description("CAD - Canadian Dollar")] CAD = 124,
    [Description("CHF - Swiss Franc")] CHF = 756,
    [Description("HKD - Hong Kong Dollar")] HKD = 344,
    [Description("NZD - New Zealand Dollar")] NZD = 554,
    [Description("VND - Vietnamese Đồng")] VND = 704

}