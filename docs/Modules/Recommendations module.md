# Recommendations Module

Provides personalized recommendations for mentees.

-   Algorithms based on reviews, ratings, and past interactions.
-   Use of ML.NET for matrix factorization (recommendation systems).
-   Analysis of user behavior (e.g., viewed profiles, mentee preferences).
-   Search by industry, programming languages, experience, rating, etc.
-   Advanced filtering (e.g., region, availability on specific dates).
-   Result sorting (by rating, reviews, experience).

**Move MenteeProfiles and MentorProfiles from Users module to this module**

# User Stories

## **1. Recommendation Mechanisms**

**Implement Weekly Recommendation Emails**
 Generate mentor suggestions using **ML.NET recommendation system**.

## **2. Mentor-Mentee Matching System**

**Story:** As a mentee, I want to **find and connect with a mentor** based on my learning needs.

### **Tasks:**

**MATCH-005 Implement Matching Algorithm with Filters**
 Add filtering by Skills, Industry, Experience, Language.
 Sort by Rating, Availability.

---

## **3. Search & Discovery**

**Story:** As a user, I want to **search for mentors efficiently**.

### **Tasks:**

**[SEARCH-002] Implement Advanced Filters for Search**
 Filter by Industry, Skills, Experience, Language.
 Implement GET /api/search/mentors?skill=C%23&experience=5.

**[SEARCH-003] Implement Sorting & Pagination**
 Sort by Rating, Experience, Availability.

---

### Story 1: Track Mentor Profile Views

**As** a system, **I want** to record each time a mentee views a mentor's profile
**So that** I can feed this data into the recommendation engine

**Tasks:**

-   **[REC-101]** Define POST /api/recommendations/events/views endpoint
    -   Payload: { menteeId: Guid, mentorId: Guid }
    -   Controller: MentorEventsController.RecordView(MentorViewEventDto dto)
-   **[REC-102]** Implement MentorViewEvent entity & EF Core mapping
-   **[REC-103]** Create IMentorEventService.RecordViewAsync() + its implementation
-   **[REC-104]** Write integration tests for view-recording endpoint

---

### Story 2: Track Mentor Bookmarks

**As** a mentee, **I want** to bookmark my favorite mentors
**So that** I can revisit them and boost them in my recommendations

**Tasks:**

-   **[REC-111]** Define POST /api/recommendations/events/bookmarks endpoint
    -   Payload: { menteeId: Guid, mentorId: Guid }
    -   Controller: MentorEventsController.RecordBookmark(MentorBookmarkDto dto)
-   **[REC-112]** Implement MentorBookmark entity & EF Core mapping
-   **[REC-113]** Create IMentorEventService.RecordBookmarkAsync() + implementation
-   **[REC-114]** Write integration tests for bookmark endpoint

---

### Story 3: Manage Mentee Preferences (CBF)

**As** a mentee, **I want** to save my preferences (industries, languages, experience, region)
**So that** recommendations can be tuned to my goals

**Tasks:**

-   **[REC-121]** Define CRUD endpoints under /api/recommendations/preferences
    -   GET /{menteeId}, POST (create), PUT /{id}, DELETE /{id}
    -   Controller: PreferencesController
-   **[REC-122]** Implement MenteePreferences entity & EF Core mapping
-   **[REC-123]** Create IPreferencesService + PreferencesService with methods:
    -   GetByMenteeAsync(menteeId), UpsertAsync(pref), DeleteAsync(id)
-   **[REC-124]** Write unit tests for preferences service + integration tests for endpoints

---

### Story 4: Aggregate Interactions (ETL)

**As** the system, **I want** to periodically aggregate views, bookmarks, ratings, bookings into MenteeMentorInteraction
**So that** the CF model has up-to-date training data

**Tasks:**

-   **[REC-131]** Implement InteractionAggregator.RunAsync() (we already scaffolded)
-   **[REC-132]** Register and schedule the background job (e.g., every 6 hours)
-   **[REC-133]** Create unit tests/mocks for InteractionAggregator logic
-   **[REC-134]** Verify MenteeMentorInteraction table population in dev environment

---

### Story 5: Train Collaborative Filtering Model

**As** the system, **I want** to train an ML.NET matrix factorization model on interaction data
**So that** I can predict mentee-mentor affinity

**Tasks:**

-   **[REC-141]** Implement CollaborativeTrainer.TrainAsync() (we scaffolded code)
-   **[REC-142]** Save the trained model to a known location (model.zip)
-   **[REC-143]** Schedule the training job (e.g., nightly)
-   **[REC-144]** Write tests to ensure model file is created and loadable

---

### Story 6: Generate Hybrid Recommendations

**As** a system, **I want** to predict CF scores, compute CBF scores, combine them, and store results
**So that** I can quickly serve finalized recommendations

**Tasks:**

-   **[REC-151]** Implement HybridScorer.GenerateRecommendationsAsync() (scaffolded)
-   **[REC-152]** Define RecommendationResult entity & EF Core mapping
-   **[REC-153]** Schedule HybridScorer to run after training completes
-   **[REC-154]** Create tests to validate combined scoring logic

---

### Story 7: Expose Recommendation API

**As** a mentee, **I want** to fetch my top-N recommendations
**So that** I can see which mentors are best matched to me

**Tasks:**

-   **[REC-161]** Define GET /api/recommendations/{menteeId}?top=N endpoint
    -   Controller: RecommendationsController.GetRecommendations(menteeId, top)
-   **[REC-162]** Map RecommendationResult RecommendationDto { mentorId, collaborativeScore, contentBasedScore, finalScore }
-   **[REC-163]** Implement pagination / sorting by inalScore desc
-   **[REC-164]** Write integration tests for recommendation endpoint

---

### Story 8: Admin CRUD for Recommendations (Optional)

**As** an admin, **I want** to view and clear cached recommendations
**So that** I can manage stale or incorrect data

**Tasks:**

-   **[REC-171]** GET /api/recommendations/admin (list all, filter by date)
-   **[REC-172]** DELETE /api/recommendations/admin/{id} (remove a single record)
-   **[REC-173]** DELETE /api/recommendations/admin?before=YYYY-MM-DD (bulk purge)
-   **[REC-174]** Secure admin endpoints with role-based authorization

---

## Summary of Endpoints

| Verb   | Route                                       | Description                             |
| ------ | ------------------------------------------- | --------------------------------------- |
| POST   | /api/recommendations/events/views           | Record a profile view                   |
| POST   | /api/recommendations/events/bookmarks       | Record a bookmark                       |
| GET    | /api/recommendations/preferences/{menteeId} | Get mentee preferences                  |
| POST   | /api/recommendations/preferences            | Create/update mentee preferences        |
| PUT    | /api/recommendations/preferences/{id}       | Update preferences                      |
| DELETE | /api/recommendations/preferences/{id}       | Delete preferences                      |
| GET    | /api/recommendations/{menteeId}             | Get top-N recommendations (query param) |
| GET    | /api/recommendations/admin                  | Admin: list all recommendation results  |
| DELETE | /api/recommendations/admin/{id}             | Admin: delete single result             |
| DELETE | /api/recommendations/admin                  | Admin: bulk delete results              |

---

# Algorithm

## GOAL

Build a **hybrid recommendation engine**:

-   Collaborative Filtering (CF) using ML.NET's MatrixFactorizationTrainer
-   Content-Based Filtering (CBF) based on mentee preferences + mentor profiles
-   Combine both into a final ranked list per mentee

## STEP-BY-STEP ALGORITHM

---

### **Step 1: Collect Interaction Data**

**Source modules:** Recommendations (Views/Bookmarks), Ratings, Bookings

#### 1.1. Pull all raw interaction events:

-   MentorViewEvent from this module
-   MentorBookmark from this module
-   Ratings from Ratings module
-   Bookings from Bookings module

#### 1.2. Assign scores to each event:

| Event Type        | Score |
| ----------------- | ----- |
| Profile viewed    | +1    |
| Mentor bookmarked | +3    |
| Session booked    | +5    |
| Session completed | +2    |
| Rating 45         | +4    |
| Rating 12         | -2    |
| No-show / cancel  | -2    |

#### 1.3. Group by (MenteeId, MentorId) and **sum** the scores.

#### 1.4. Write/Upsert into MenteeMentorInteraction table:

`csharp
new MenteeMentorInteraction
{
    MenteeId = ..., MentorId = ..., Score = aggregatedScore
}
`

---

### **Step 2: Prepare CF Training Dataset**

#### 2.1. Query all rows from MenteeMentorInteraction

#### 2.2. Map to ML.NET model format:

`csharp
public class MenteeMentorRatingData
{
    public string MenteeId { get; set; }
    public string MentorId { get; set; }
    public float Label { get; set; }
}
`

#### 2.3. Normalize scores (optional, 05 range)

---

### **Step 3: Train CF Model via ML.NET**

#### 3.1. Use MatrixFactorizationTrainer:

\\\csharp
var pipeline = mlContext.Recommendation().Trainers.MatrixFactorization(
new MatrixFactorizationTrainer.Options
{
MatrixColumnIndexColumnName = nameof(MenteeMentorRatingData.MenteeId),
MatrixRowIndexColumnName = nameof(MenteeMentorRatingData.MentorId),
LabelColumnName = nameof(MenteeMentorRatingData.Label),
NumberOfIterations = 20,
ApproximationRank = 100
});
\\\

#### 3.2. Train the model

#### 3.3. Save the model for predictions

---

### **Step 4: Generate CF Predictions**

#### 4.1. For each mentee:

-   Predict score for all mentors
-   Save top-N results into RecommendationResult with CollaborativeScore

`csharp
new RecommendationResult
{
    MenteeId = ..., MentorId = ..., CollaborativeScore = predictedScore
}
`

---

### **Step 5: Compute CBF Scores**

#### 5.1. For each mentee:

-   Fetch MenteePreferences
-   Fetch all mentors' profile attributes from UsersModule

#### 5.2. For each mentor, compute a similarity score:

`csharp
CBFScore = +2 for matching industry
         +1 per overlapping programming language
         +1 if region matches
         +1 if preferred communication language matches
         +1 if experience >= desired
`

#### 5.3. Store into same RecommendationResult row or join:

`csharp
rec.ContentBasedScore = cbfScore;
`

---

### **Step 6: Combine CF + CBF**

For each RecommendationResult, calculate:

`csharp
rec.FinalScore = 0.7f * rec.CollaborativeScore + 0.3f * rec.ContentBasedScore;
`

Update DB accordingly.

---

### **Step 7: Serve Recommendations**

Expose endpoint:

`http
GET /recommendations?menteeId=...
`

Return top-N mentors by FinalScore from RecommendationResult.

---

## Optional: Automate via Background Job

Run steps 16 periodically (e.g., nightly):

-   Step 12: data ETL job
-   Step 34: training job
-   Step 56: scoring job
