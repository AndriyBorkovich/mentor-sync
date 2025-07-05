 Надає доступ до додаткових ресурсів.
- Завантаження та збереження навчальних матеріалів (текст, фото, посилання, документами).
- База знань із рекомендованими статтями, документами, відео.
- Пошук статтей (full text search)
## User stories

### 🎯 Epic: Learning Materials CRUD & Metadata

#### Story **LM-101** – Create a Learning Material

**As** a mentor  
**I want** to create a new learning material (article/book/course)  
**So that** I can share resources with mentees

- **LM-101.1**: Define `CreateMaterialDto { Title, Description, Type, ContentMarkdown, MetadataJson, TagIds[] }`
    
- **LM-101.2**: Implement `ILearningMaterialService.CreateAsync(CreateMaterialDto)`
    
- **LM-101.3**: Expose `POST /api/materials` in `MaterialsController`
    
- **LM-101.4**: Unit tests for service validation (required fields, markdown length)
    
- **LM-101.5**: Integration test for endpoint and DB persistence
    

#### Story **LM-102** – Read & List Materials

**As** any user  
**I want** to fetch a material by ID and list my mentors’ materials  
**So that** I can browse available resources

- **LM-102.1**: Define `MaterialDto` with all fields + `TagNames[]`
    
- **LM-102.2**: Implement `GetByIdAsync(int id)` and `ListByMentorAsync(int mentorId)`
    
- **LM-102.3**: Expose `GET /api/materials/{id}` and `GET /api/materials?mentorId=`
    
- **LM-102.4**: Tests for correct mapping and paging
    

#### Story **LM-103** – Update a Learning Material

**As** the material owner  
**I want** to update title, content, tags, etc.  
**So that** I can improve or correct my resource

- **LM-103.1**: Define `UpdateMaterialDto { Id, … }`
    
- **LM-103.2**: Implement `UpdateAsync(UpdateMaterialDto)` in service
    
- **LM-103.3**: Expose `PUT /api/materials/{id}`
    
- **LM-103.4**: Authorization: only mentor who created it
    
- **LM-103.5**: Tests for ownership and data changes
    

#### Story **LM-104** – Delete a Learning Material

**As** the material owner or admin  
**I want** to delete a material  
**So that** outdated or irrelevant resources are removed

- **LM-104.1**: Implement `DeleteAsync(int id)` in service
    
- **LM-104.2**: Expose `DELETE /api/materials/{id}`
    
- **LM-104.3**: Cascade delete of `MaterialTag` & `MaterialAttachment`
    
- **LM-104.4**: Tests for soft/hard deletion and cleanup
    

---

### 🎯 Epic: Tag Management

#### Story **LM-111** – Create & List Tags

**As** a mentor/admin  
**I want** to manage tags (e.g., “C#”, “Data Science”)  
**So that** materials can be categorized

- **LM-111.1**: Define `TagDto { Id, Name }`
- **LM-111.2**: Expose `GET /api/materials/tags` and `POST /api/materials/tags`
- **LM-111.3**: Implement `ITagService.CreateAsync` and `ListAsync`
#### Story **LM-112** – Assign/Remove Tags on Materials

**As** a mentor  
**I want** to tag my materials  
**So that** they show up in category searches

- **LM-112.1**: Extend `CreateMaterialDto`/`UpdateMaterialDto` with `TagIds[]`
- **LM-112.2**: Service logic to upsert `MaterialTag` join table

### 🎯 Epic: Attachment Handling

#### Story **LM-121** – Upload & List Attachments

**As** a mentor  
**I want** to upload files (images, PDFs) for a material  
**So that** learners can download supplemental resources
- **LM-121.1**: Expose `POST /api/materials/{id}/attachments` accepting multipart/form-data
- **LM-121.2**: Implement `IAttachmentService.UploadAsync(int materialId, IFormFile file)` 
    - Upload to Azure Blob Storage, return `BlobUri`        
    - Persist `MaterialAttachment` record
- **LM-121.3**: Expose `GET /api/materials/{id}/attachments`
#### Story **LM-122** – Delete Attachments

**As** a mentor  
**I want** to remove an attachment  
**So that** outdated files are cleaned up

- **LM-122.1**: Implement `DeleteAsync(int attachmentId)` in service (remove blob + DB)
- **LM-122.2**: Expose `DELETE /api/materials/{id}/attachments/{attachmentId}`
- **LM-122.3**: Tests for blob deletion and record removal

---

