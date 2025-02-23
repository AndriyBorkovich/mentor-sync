param isProd bool
param communicationServiceName string
param emailServiceName string
param keyVaultName string
param location string // need to pass to make the module work

// Email Communication Service
resource emailService 'Microsoft.Communication/emailServices@2023-03-31' = {
  name: emailServiceName
  location: 'Global'
  properties: {
    dataLocation: 'UnitedStates'
  }
}

// Email Communication Services Domain (Azure Managed)
resource emailServiceAzureDomain 'Microsoft.Communication/emailServices/domains@2023-03-31' = if (!isProd) {
  parent: emailService
  name: 'AzureManagedDomain'
  location: 'Global'
  properties: {
    domainManagement: 'AzureManaged'
    userEngagementTracking: 'Disabled'
  }
}

// SenderUsername (Azure Managed Domain)
resource senderUserNameAzureDomain 'Microsoft.Communication/emailServices/domains/senderUsernames@2023-03-31' = if (!isProd) {
  parent: emailServiceAzureDomain
  name: 'donotreply'
  properties: {
    username: 'DoNotReply'
    displayName: 'DoNotReply'
  }
}

// Email Communication Services Domain (Customer Managed)
resource emailServiceCustomDomain 'Microsoft.Communication/emailServices/domains@2023-03-31' = if (isProd) {
  parent: emailService
  name: 'mentorsync.dev'
  location: 'Global'
  properties: {
    domainManagement: 'CustomerManaged '
    userEngagementTracking: 'Disabled'
  }
}

// SenderUsername (Customer Managed Domain)
resource senderUserNameCustomDomain 'Microsoft.Communication/emailServices/domains/senderUsernames@2023-03-31' = if (isProd) {
  parent: emailServiceCustomDomain
  name: 'donotreply'
  properties: {
    username: 'DoNotReply'
    displayName: 'DoNotReply'
  }
}

// Link the correct domain based on the environment
var emailServiceResource = isProd ? emailServiceCustomDomain.id : emailServiceAzureDomain.id

// Communication Service
resource communcationService 'Microsoft.Communication/communicationServices@2023-03-31' = {
  name: communicationServiceName
  location: 'Global'
  properties: {
    dataLocation: 'UnitedStates'
    linkedDomains: [
      emailServiceResource
    ]
  }
}

// Reference the Key Vault secret for the connection string
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' existing = {
  name: keyVaultName
}

resource csConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: 'cs-connectionString'
  properties: {
    value: communcationService.listKeys().primaryConnectionString
  }
  parent: keyVault
}
