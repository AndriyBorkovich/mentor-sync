@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

resource mentor_sync_storage 'Microsoft.Storage/storageAccounts@2024-01-01' = {
  name: take('mentorsyncstorage${uniqueString(resourceGroup().id)}', 24)
  kind: 'StorageV2'
  location: location
  sku: {
    name: 'Standard_GRS'
  }
  properties: {
    accessTier: 'Hot'
    allowSharedKeyAccess: false
    minimumTlsVersion: 'TLS1_2'
    networkAcls: {
      defaultAction: 'Allow'
    }
  }
  tags: {
    'aspire-resource-name': 'mentor-sync-storage'
  }
}

resource blobs 'Microsoft.Storage/storageAccounts/blobServices@2024-01-01' = {
  name: 'default'
  parent: mentor_sync_storage
}

output blobEndpoint string = mentor_sync_storage.properties.primaryEndpoints.blob

output queueEndpoint string = mentor_sync_storage.properties.primaryEndpoints.queue

output tableEndpoint string = mentor_sync_storage.properties.primaryEndpoints.table

output name string = mentor_sync_storage.name