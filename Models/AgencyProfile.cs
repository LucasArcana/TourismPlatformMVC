using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Agency
{
    public int AgenId { get; set; }
    public string AgencyName { get; set; }
    public string Desc { get; set; }
    public string ContEmail { get; set; }
    public string PhoneNo { get; set; }
    public string Address { get; set; }
}