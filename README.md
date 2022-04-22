# **HttpLoggerMiddleware**

Middleware that records all the data of the incoming requests and outgoing responses into a *.json file.  
Recorded attributes: TimeStamp, Route, Method, QueryParameters, Headers, Payload.

If you find this useful, then please hit the star button on my GitHub repo
**https://github.com/pforro/HttpLoggerMiddleware.git**

## **Usage**

- Install the NuGet package
	 - > **https://www.nuget.org/packages/HttpLoggerMiddleware/1.0.0**
- Add the following namespace to the Program.cs file:
	 - > **using HttpLoggerMiddleware;**
 - Add the following line to the request pipeline in the Program.cs file:
	 -  > **app.UseHttpLogger();**
 - Build and Run the web application, send some HTTP requests
 - You can find the log.json file in the following path:
	 - > **[WorkingDirectory]/HttpLogs/log.json**
## **Author**
- Peter Cs. Forro
- Budapest, Hungary, 2022