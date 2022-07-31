# AzureAppConfigurationPOC


Azure App configuration is the centralized storage and management of App settings configured in the applications.

Apps sometimes store config as constants in the code. This is a violation of twelve-factor, which requires strict separation of config from code. Config varies substantially across deploys, code does not.

By integrating Azure App configuration we can follow "12 Factor" in our architecture.

I have integrated the Azure app configuration in the below Project types

<b>1. MVC architecture in .Net 4.7.2</b>

<b>2. Web Api in .Net 6</b>

<b>3. Azure Function in .Net 6</b>

    a. In Process
    b. Isoloated Process
 

For running the above solutions, please follow the below steps to integrate Azure App configuration.

## Create the Azure App configuration service in the Azure portal 

https://docs.microsoft.com/en-us/azure/azure-app-configuration/quickstart-dotnet-core-app?tabs=linux

Choose "Free" tier, if you are using it for POC 

Add the below configurations in the configuration explorer

![image](https://user-images.githubusercontent.com/12987861/182019488-a5b9daa7-220b-4249-bbee-7184fa065a01.png)

![image](https://user-images.githubusercontent.com/12987861/182019445-56a3a2fe-e5f3-401d-8c06-b02499096cb9.png)


## App Integration 

1. Copy the Connection string from the "Azure App Configuration" and paste in the App setting 

### Azure Function and Web Api
![image](https://user-images.githubusercontent.com/12987861/182018716-afa7585c-5139-4afb-91d0-dea681ac58b7.png)

### MVC app with .Net 4.7.2
![image](https://user-images.githubusercontent.com/12987861/182019327-24cf7ec6-42a8-4b80-b18b-16c89dcf1ba1.png)

## How it works

#### MVC App

In the Global.asax file, I added the code to connect the azure app configuration and getting the app setting from it in the Application Start method.

"refreshAll" configuration will refresh the app configuration without stopping your solution. Once you update the existing configuration and update the "refreshAll" configuration, It will update the application configuration without restarting your app.

For refreshing the configuration, I have added the refresher call in the application begin request. It will sync with your azure app configuration for each reqeust. If you dont need to refresh the configuration, you can skip this step and app configuration will be refreshed when application ran again.
![image](https://user-images.githubusercontent.com/12987861/182019558-da49a0c8-563c-4fd9-bae7-59663119f649.png)

![image](https://user-images.githubusercontent.com/12987861/182019588-a74f6bdb-8089-4a4c-86c2-cbfcae9d0aaf.png)


### .Net Core App with .Net 6

In the program.cs file configure the Azure app configuration and get the setting based on the section you want. We can configure the cache and refresh in the same configuration as we did for mvc.

![image](https://user-images.githubusercontent.com/12987861/182019912-be818490-41cc-4b75-8c5b-8d056dbddb88.png)

Except Inprocess, we have the middleware available in .Net Isolated Process and web api, so it will take care of refreshing the configuration.

![image](https://user-images.githubusercontent.com/12987861/182020140-a3c2d433-4ae1-419a-9bea-8ba89eb2f244.png)


In InProcess you must make a call in each function to refresh the configuration

![image](https://user-images.githubusercontent.com/12987861/182020122-dda75c32-495c-45ef-8860-17ed6928a7f2.png)


When the application run, the app configuration values will be pulled from the Azure app configuraition and bind it with the IConfiguration which we can injected in the Controller

![image](https://user-images.githubusercontent.com/12987861/182020289-b518fc0f-5feb-4b18-92b1-b9fd20457c70.png)

![image](https://user-images.githubusercontent.com/12987861/182020311-a8609746-ccf3-4a5d-978b-4040c05c6e45.png)

For best practice, we can use Option pattern for using class as strongly typed access to groups of related settings

![image](https://user-images.githubusercontent.com/12987861/182020332-f41d12c8-72d5-4e35-bf72-6c68b0faf75d.png)


