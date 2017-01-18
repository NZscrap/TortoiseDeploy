# TortoiseDeploy
Simplifying deployments, for when automated deployments are nothing but a pipe dream.

##Installation
Currently, no pre-built binaries are available.

You'll need to download the repository and build the project in Visual Studio.

##Configuration
Configuration is handled through [config.json](TortoiseDeploy/config.json)
The top level configuration options are as follows:

Setting | Meaning
------- | -------
MergeToolPath | Path to a diff tool binary. This tool must accept two command line parameters representing the two files to compare.
RepositoryRoot | The local path of the repository files. This is used to support relative paths in Deployment Mappings, see below for more details.
RemoteDeploymentGroupURL | Optional. This specifies a url to a json file that contains a list of DeploymentGroups (here is an [example](TortoiseDeploy/remote.json)) If this is specified, TortoiseDeploy will attempt to fetch the DeploymentGroups from the url on startup. It will then **overwrite** the current list of DeploymentGroups with what was retrieved from the URL.
DeploymentGroups | A list of all Deployment Groups (see below). Note that if the RemoteDeploymentGroupURL is specified, this will be **overwritten** on startup, this is to ensure that everyone using a hosted config will use the same DeploymentGroups at all times.

###Deployment Group
Deployment Groups are used to specify how TortoiseDeploy should connect to a deployment target, where files in the repository should be deployed, and what we should do after the deployment is completed.
The possible configuration options are outlined below:

Setting | Meaning
------- | -------
Name | User-friendly name for the deployment group. This is just a courtesy to make the config file more readable.
PreDeploymentScript | Path to an executable file that should be run before any deployments for this group. If no directories are specified, we'll search the same folder the TortoiseDeploy binary is in.
PreDeploymentArguments | Arguments to pass to the PreDeploymentScript executable.
PostDeploymentScript | Path to an executable file that should be run after all deployments in this group. As with the PreDeploymentScript, we search the same folder as the TortoiseDeploy executable if no folder is specified.
PostDeploymentArguments | Arguments to pass to the PostDeploymentScript executable.
DeploymentMappings | List of DeploymentMapping objects, see below.
DeploymentMapping | A mapping from a Source location to a Destination. Source can be a full path to a folder or file, or a relative path starting with a backslash (\\) - relative paths will be relative to the RepositoryRoot. The Destination path should be the full path of a folder to deploy files into.


##Usage
TortoiseDeploy is designed for use from TortoiseSVN as a client side post-commit hook.

To do this, open TortoiseSVN Settings -> Hook Scripts
* Create a new hook script
* Set the Hook Type to Post-Commit Hook
* Set the "Command Line To Execute" to point to the TortoiseDeploy executable
* Ensure the "Hide the script while running" checkbox is **not** checked
