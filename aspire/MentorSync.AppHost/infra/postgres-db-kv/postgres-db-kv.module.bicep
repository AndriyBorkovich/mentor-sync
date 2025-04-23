@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

resource postgres_db_kv 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: take('postgresdbkv-${uniqueString(resourceGroup().id)}', 24)
  location: location
  properties: {
    tenantId: tenant().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
    enableRbacAuthorization: true
  }
  tags: {
    'aspire-resource-name': 'postgres-db-kv'
  }
}

output vaultUri string = postgres_db_kv.properties.vaultUri

output name string = postgres_db_kv.name