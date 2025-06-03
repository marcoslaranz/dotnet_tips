# Adding Swagger to your code.

## Create a project


	dotnet new webapi -n Swagger

## Add the NuGet package to your project.

	cd Swagger
 
  dotnet add package Swashbuckle.AspNetCore
 

## Modify your Program.cs.

![image](https://github.com/user-attachments/assets/51a7f87a-2b77-431d-a3b9-7a148c453ade)
 

## Compile and test.

![image](https://github.com/user-attachments/assets/9b4249ed-6a49-4e35-b93e-89216ff9cdff)



![image](https://github.com/user-attachments/assets/3f24455d-37fa-4798-973a-14cd471b37e6)


# Note: 
If you don't add Swagger, you can still view the OpenAPI specification. The default minimal API template used by 'dotnet' automatically includes the OpenAPI functionality in your code (in Program.cs). When you run your program, you will be able to see this specification by using the following:



 
 ```sh
 http://localhost:<PORT>/openapi/v1.json
```
 


---

 
 




