namespace App.Application.Features.Products.Dto;



public record ProductDto(int Id,string Name ,decimal Price ,int Stock ,int CategoryId);

// use record.. i wanna compare only values 

//public record ProductDto
//{
//    // i use init cause i dont want that nobody change my propert after response .. after instance is created . the property never changes
//    public int Id { get; init; }
//    public string Name { get; init; }

//    public decimal Price { get; init; }
//    public int Stock { get; init; }
//}
