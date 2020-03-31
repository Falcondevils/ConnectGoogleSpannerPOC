# ConnectGoogleSpannerPOC
The repository is a .net core api basic POC to connect to a google spanner database. The solution is a basic demo to show how to connect to a google spanner database and perform basic operations

# PreRequisites
1)There should be cloud SDK installed in the development environment where the solution will be running
2) I used cloud SDK default aurthentication to authenticate GCP credential. So you might have to follow the same.
Do below to set up the authentication
  a) Make sure Cloud SDK is installed in your system(windows)
  b) In the Cloud Console, go to the Create service account key page.
  Go to the Create Service Account Key page
  From the Service account list, select New service account.
  In the Service account name field, enter a name.
  From the Role list, select Project > Owner.

  Note: The Role field authorizes your service account to access resources. You can view and change this field later    by   using the Cloud Console. If you are developing a production app, specify more granular permissions than          Project Owner. For more information, see granting roles to service accounts.
  Click Create. A JSON file that contains your key downloads to your computer.
  
  c) set environment variable using below
  With PowerShell:

    $env:GOOGLE_APPLICATION_CREDENTIALS="[PATH]"

    For example:

    $env:GOOGLE_APPLICATION_CREDENTIALS="C:\Users\username\Downloads\[FILE_NAME].json"

    With command prompt:

    set GOOGLE_APPLICATION_CREDENTIALS=[PATH]
    
   d) Make sure to run below command in your command prompt to make sure your cloud SDK is in default authentication    mode
   gcloud auth application-default login
   
   
# About Solution

1) All the cloud spanner code is in OfferingController.cs. That file is enough to go throufgh all the code.
2) Change the constant variables in that file for your Project Id, Spanner Instance ID and database
3) Make sure your spanner table is in sync with the table in this solution
4) Download spanner dll from nuget package manager as necessary
  
