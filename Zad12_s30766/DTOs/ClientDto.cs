namespace Zad12_s30766.DTOs;

public class ClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class AddClientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    public DateTime? PaymentDate { get; set; }
}