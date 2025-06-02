namespace Tutorial12.DTOs;

public class TripsInfoDTO
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDTO> trips { get; set; }
}