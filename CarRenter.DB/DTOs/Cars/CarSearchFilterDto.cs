namespace CarRenter.DB.DTOs.Cars;

public class CarSearchFilterDto
{
    public string? Location { get; set; }
    public string? Brand { get; set; }     
    public string? Model { get; set; }
    public string? Color { get; set; }
    
    public DateTime? StartDate { get; set; }   
    public DateTime? EndDate { get; set; } 
    
    public decimal? HourlyPrice { get; set; }
    
    public bool? IsAvailable { get; set; } = true;
}