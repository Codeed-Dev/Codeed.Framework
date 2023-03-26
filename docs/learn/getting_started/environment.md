# Environment

The environment section of this documentation will guide you through the process of creating Firebase Authentication for your web app, as well as setting up the databases Postgres and MongoDB. Firebase Authentication allows you to easily add user authentication and authorization to your web app, while Postgres and MongoDB are powerful and widely-used databases that can store and manage your app's data. By following the steps outlined in this section, you'll be able to create a secure and reliable authentication system for your web app and set up your databases to efficiently manage your data. We'll cover everything from creating a Firebase project and obtaining authentication keys to setting up and configuring Postgres and MongoDB, so that you can get up and running quickly and easily. Let's get started!

## Create Firebase Project

- [ ] Go to the Firebase Console at [https://console.firebase.google.com/](https://console.firebase.google.com/) and sign in with your Google account.

- [ ] Click on the "Add Project" button and enter "Learn" as the project name.

- [ ] Follow the prompts to set up your project

- [ ] Once your project is created, click on the "Web" button to add a web app to your project.

- [ ]  Give your web app a name and click on the "Register App" button.

You will be given a set of configuration keys for your web app, including a Firebase configuration object that includes your API key, auth domain and projectId:

```javascript
{
  apiKey: "...",
  authDomain: "learn-8bddb.firebaseapp.com",
  projectId: "learn-8bddb",
  storageBucket: "learn-8bddb.appspot.com",
  messagingSenderId: "...",
  appId: "..."
};
```

## Databases

- [ ] In your Learn.Web project directory, create a new file called `docker-compose.yml`.

- [ ] Open the docker-compose.yml file in a text editor and copy-paste the following code:

```yml
version: '3.1'

services:

   mongo:
     image: mongo
     container_name: mongo-db-learn
     restart: always
     ports:
       - 27017:27017
     environment:
       MONGO_INITDB_ROOT_USERNAME: root
       MONGO_INITDB_ROOT_PASSWORD: mongodb
   
   mongo-express:
     image: mongo-express
     container_name: mongo-express-learn
     restart: always
     ports:
       - 8081:8081
     environment:
       ME_CONFIG_MONGODB_ADMINUSERNAME: root
       ME_CONFIG_MONGODB_ADMINPASSWORD: mongodb
       ME_CONFIG_MONGODB_URL: mongodb://root:mongodb@mongo:27017/

   postgredb:
     image: postgres
     container_name: postgres-learn
     ports:
       - 5432:5432
     environment:
       POSTGRES_PASSWORD: postgres

```

- [ ] Save the docker-compose.yml file.

- [ ] Open a terminal or command prompt and navigate to the directory where the docker-compose.yml file is located.

- [ ] Run the following command to start the containers:

```powershell
docker-compose up -d
```

This command will download the necessary images and create the MongoDB and Postgres containers.

