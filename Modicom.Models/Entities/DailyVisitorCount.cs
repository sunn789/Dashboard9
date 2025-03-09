using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Modicom.Models.Entities;
public class DailyVisitors
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int VisitorCount { get; set; }
}
