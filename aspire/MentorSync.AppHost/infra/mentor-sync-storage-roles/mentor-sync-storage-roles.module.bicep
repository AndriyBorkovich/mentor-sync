@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param mentor_sync_storage_outputs_name string

param principalType string

param principalId string

resource mentor_sync_storage 'Microsoft.Storage/storageAccounts@2024-01-01' existing = {
  name: mentor_sync_storage_outputs_name
}

resource mentor_sync_storage_StorageBlobDataContributor 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(mentor_sync_storage.id, principalId, subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'))
  properties: {
    principalId: principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'ba92f5b4-2d11-453d-a403-e96b0029c9fe')
    principalType: principalType
  }
  scope: mentor_sync_storage
}

resource mentor_sync_storage_StorageTableDataContributor 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(mentor_sync_storage.id, principalId, subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3'))
  properties: {
    principalId: principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '0a9a7e1f-b9d0-4cc4-a60d-0319b160aaa3')
    principalType: principalType
  }
  scope: mentor_sync_storage
}

resource mentor_sync_storage_StorageQueueDataContributor 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(mentor_sync_storage.id, principalId, subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '974c5e8b-45b9-4653-ba55-5f855dd0fb88'))
  properties: {
    principalId: principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '974c5e8b-45b9-4653-ba55-5f855dd0fb88')
    principalType: principalType
  }
  scope: mentor_sync_storage
}