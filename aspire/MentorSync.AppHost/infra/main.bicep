targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention, the name of the resource group for your application will use this name, prefixed with rg-')
param environmentName string

@minLength(1)
@description('The location used for all deployed resources')
param location string

@description('Id of the user or app to assign application roles')
param principalId string = ''

@secure()
param mongo_password string
@secure()
param mongo_username string
@secure()
param postgre_password string
@secure()
param postgre_username string

var tags = {
  'azd-env-name': environmentName
}

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${environmentName}'
  location: location
  tags: tags
}
module resources 'resources.bicep' = {
  scope: rg
  name: 'resources'
  params: {
    location: location
    tags: tags
    principalId: principalId
  }
}

module communication_service 'communication-service/communication-service.module.bicep' = {
  name: 'communication-service'
  scope: rg
  params: {
    communicationServiceName: 'cs-mentorsync-dev'
    emailServiceName: 'es-mentorsync-dev'
    isProd: false
    keyVaultName: resources.outputs.SERVICE_BINDING_KVA12EFD91_NAME
    location: location
  }
}
module postgres_db 'postgres-db/postgres-db.module.bicep' = {
  name: 'postgres-db'
  scope: rg
  params: {
    administratorLogin: postgre_username
    administratorLoginPassword: postgre_password
    keyVaultName: postgres_db_kv.outputs.name
    location: location
  }
}
module postgres_db_kv 'postgres-db-kv/postgres-db-kv.module.bicep' = {
  name: 'postgres-db-kv'
  scope: rg
  params: {
    location: location
  }
}
module postgres_db_kv_roles 'postgres-db-kv-roles/postgres-db-kv-roles.module.bicep' = {
  name: 'postgres-db-kv-roles'
  scope: rg
  params: {
    location: location
    postgres_db_kv_outputs_name: postgres_db_kv.outputs.name
    principalId: resources.outputs.MANAGED_IDENTITY_PRINCIPAL_ID
    principalType: 'ServicePrincipal'
  }
}

output MANAGED_IDENTITY_CLIENT_ID string = resources.outputs.MANAGED_IDENTITY_CLIENT_ID
output MANAGED_IDENTITY_NAME string = resources.outputs.MANAGED_IDENTITY_NAME
output AZURE_LOG_ANALYTICS_WORKSPACE_NAME string = resources.outputs.AZURE_LOG_ANALYTICS_WORKSPACE_NAME
output AZURE_CONTAINER_REGISTRY_ENDPOINT string = resources.outputs.AZURE_CONTAINER_REGISTRY_ENDPOINT
output AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID string = resources.outputs.AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID
output AZURE_CONTAINER_REGISTRY_NAME string = resources.outputs.AZURE_CONTAINER_REGISTRY_NAME
output AZURE_CONTAINER_APPS_ENVIRONMENT_NAME string = resources.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_NAME
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = resources.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_ID
output AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = resources.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN
output SERVICE_MONGO_VOLUME_MONGODATA_NAME string = resources.outputs.SERVICE_MONGO_VOLUME_MONGODATA_NAME
output SERVICE_BINDING_KVA12EFD91_ENDPOINT string = resources.outputs.SERVICE_BINDING_KVA12EFD91_ENDPOINT
output SERVICE_BINDING_KVA12EFD91_NAME string = resources.outputs.SERVICE_BINDING_KVA12EFD91_NAME
output AZURE_VOLUMES_STORAGE_ACCOUNT string = resources.outputs.AZURE_VOLUMES_STORAGE_ACCOUNT
output POSTGRES_DB_KV_VAULTURI string = postgres_db_kv.outputs.vaultUri
