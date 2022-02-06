# WeatherService
This is a sample service created which will communicate with https://openweathermap.org/current in order to fetch weather details for provided city names.

## Problem Statement 
User needs a way to fetch latest weather data for cities. Instead of fetching data from https://openweathermap.org/current for one city at a time, user wants to send a file with list of city names to service. Service should return back data in another file with city name followed by weather information.

## Delieverables
1. User will create a text file with city names he/she wants to fetch data for. City names should be separated by pipe ('|') operator.
2. File will be uploaded to service. We can use Postman or any other available tool for same.
3. Service will get back text file with column names and valid cities weather data.
4. For same day if user trying to download file multiple time, new file downloaded will ask user to replace existing file. File will have current date as a part of it's name.

## Details

### How is it Developed
1. Application is developed in VS 2019 using .NET CORE 3.1.
2. Solution name is WeatherService.sln. Consist of 5 different projects. Weatherservice is API Project and also startup project of this solution.
![image](https://user-images.githubusercontent.com/98891535/152676657-583a4298-1f9a-4e8f-811f-8e209dc520c5.png)

3. WeatherService contains APIs Controllers which will act as an entry point for user. They will server data to end users.

![image](https://user-images.githubusercontent.com/98891535/152676742-05f43fd8-2340-4ae9-a120-5c66b2e322c4.png)

4. WeatherService.Domain : Contains Service layer. Weather and authentication services will reside here.

![image](https://user-images.githubusercontent.com/98891535/152676724-e96567da-730a-4bb8-8e72-07e21d15461b.png)

5. WeatherService.Data : This project will hold different repositories (weather and authntication), model classes, validation classes etc. In normal application this layer talks to DB and performs DB operations. In our case it will take care of Weatherservice communication.

![image](https://user-images.githubusercontent.com/98891535/152676780-4305e44c-c605-43e2-b234-37b06bfac862.png)

6. WeatherService.Common: Contains commmon services used by all other projects. Like Constants, extension methods etc. Other projects can be dependent on this but this project will have no dependency on other projects.

![image](https://user-images.githubusercontent.com/98891535/152676887-71d326ab-8d13-435f-8189-98f2e73791d1.png)

7. WeatherService.Unit : Unit tests will reside in this project. This project depends on all other projects but no project has dependency on this one.

![image](https://user-images.githubusercontent.com/98891535/152676984-71db9a70-9fe1-40e4-8177-22a235efe499.png)

#### Different Parts
1. WeatherService.Controllers.AuthenticationController : This will be used to fetch authentication token for valid user. It will authenticate user and create jwt token for user. User credentials are present in appsettings.json file. Property name is user name and Password property value is password for that user. For example in below image, user name is "User" and password is "userPassword".

![image](https://user-images.githubusercontent.com/98891535/152677095-d93dc6e2-9f04-4794-b45f-bf1ab7e6abba.png)

2. WeatherService.Domain.Service.AuthentiationService : Will invoke corresponding repository in order to validate user and get back token for user.
3. WeatherService.Data.Repository.AuthenticationRepository : Actual code where user will be validated and token will be sent to end user.
4. WeatherService.Data.Validation.ParentUserValidation : Code to validate user credentials from appsettings.json

5. WeatherService.Controllers.WeatherController : Contains 2 ation methods. One will accept city names separated by pipe operator and other will acccept text file which should contain **city names separated by pipe operator**.
6. WeatherService.Domain.Service.WeathersService : Service layer which will invoke IWeatherRepository in order to fetch weather details for given city names.
7. WeatherService.Data.Repository.WeatherRepository : Repository layer which calls api.openweathermap.org in order to fetch cities data. We have used TPL here in order to fetch data for different cities at the same time on different threads/cores. This will improve performance as doesn't have to use normal foreach to feth one city data at a time. MemoryCahe is also used to store fetched data in cache for 30 mins, assuming weather won't change much in span of 30 mins. Had planned to use Circuit Breaker architecure but unfortunately due to time constraint couldn't implement it.
8. WeatherService.Domain.ModelToTextConverter : Returns Weather data fetched in form of memory stream in order to create text file from it.
9. WeatherService.Unit.AuthenticationControllerTests : Unit tests for AuthenticationController.
10. WeatherService.Unit.WeatherServiceControllerTests : Unit tests for WeatherController.

### How to Build
1. Clone this repository.
2. Open VS 2019 in admin mode. Open solution WeatherService.sln. Clean and build it.
3. Press F5 in order to run it using IIS Express.
4. There is no hosting mode mentioned in project file. Hence by default it will use Out of process hosting where in build Kestrel web server will be used as internal server to serve requests. Whereas IIS Express will act as external web server.
5. If you get an error saying 'Unable to connect to web server IIS Express', go to launchsettings.json and try to change ssl and normal port numbers.  

###  How to Use
1. We will see how to use this service using Postman tool.
2. Run application from VS2019.
3. It should start IIS Express. Check SSL Port number as shown below in order to ues this out of process hosted api.

![image](https://user-images.githubusercontent.com/98891535/152677815-5ab11a6b-7250-41ff-b5d5-0c8d9a20c6d4.png)

4. Open postman tool and enter above starting part of URL. Followed by api/authentication/login (https://localhost:44318/api/authentication/login) in order to fetch authentication token. request type should be POST as we will be sending user credentials in body. Body type should ne JSON.

![image](https://user-images.githubusercontent.com/98891535/152677940-f66babba-9427-4aa2-b7c2-a64e3fdee433.png)

![image](https://user-images.githubusercontent.com/98891535/152677890-326c8f3c-b0d5-44e0-b9cf-223ee4ac4900.png)

5. Copy this token and Open new request. Type should be post here as we will be sending file. POST --> https://localhost:44318/api/weather-data/. 

![image](https://user-images.githubusercontent.com/98891535/152678051-5911ef42-6d12-4892-8411-ae95a599ec03.png)

![image](https://user-images.githubusercontent.com/98891535/152678071-a260a5d9-8096-4ad5-a509-f11b52efc563.png)

6. Go to Authorization tab of request in postman. Select type as Bearer Token and paste copied token from previous login API response in Token text box.

![image](https://user-images.githubusercontent.com/98891535/152678127-e90bee04-70ca-41a8-ab6a-1b51662dd1e1.png)

This will add new header in Request as 'Authorization' with value as 'Bearer ' + token. JWT Bearer authentication is being used.

![image](https://user-images.githubusercontent.com/98891535/152678177-c950b813-0883-429d-bf93-6beff979880f.png)

Create new text file in such a way that it will contain city names separated by pipe operator 

![image](https://user-images.githubusercontent.com/98891535/152678216-b2bf910a-e31e-4fb2-b278-491872961f8f.png)

Go back to postman. Select body tab under request. Select form-data radio button. Enter key name as file. On Mousehover it will ask to select input type. Select file and upload Input text file.

![image](https://user-images.githubusercontent.com/98891535/152678293-ee8f1f17-8080-4b6c-88f0-dc2723d58e3e.png)

Should get back valid city names weather data

![image](https://user-images.githubusercontent.com/98891535/152678326-1a87717a-5067-4df8-8d56-2ddf28009b99.png)

This will get back raw data. API endpoint returns back it as Fileresult. In order to download it as file we need to use send and download. It will automatically generate file name with date part appended. Will ask for saving.

![image](https://user-images.githubusercontent.com/98891535/152678417-d6a9311e-c39c-4394-81cf-037e45e6ecf9.png)

![image](https://user-images.githubusercontent.com/98891535/152678468-cf1ac69c-96ec-4280-8f6f-a4e237e643dc.png)

Open downloaded file. 

![image](https://user-images.githubusercontent.com/98891535/152678502-a34c3476-654a-4566-b709-c38f0e7af10c.png)

In order to see this data in proper format. Open new excel file. Copy and paste this data in it. This will put data properly in separate columns of excel, no need of extra formatting. Didn't get time to generate excel file.

![image](https://user-images.githubusercontent.com/98891535/152678556-78921899-d17d-4960-8a01-34794a6a511d.png)






