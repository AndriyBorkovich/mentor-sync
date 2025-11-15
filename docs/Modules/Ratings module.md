# Ratings Module

System for rating mentors by mentees and collecting reviews.

-   Leaving ratings (15 stars).
-   Writing text reviews.
-   Also includes rating of mentor's articles (separately)

## Core Features

1. **Mentor Reviews**
    - Submit a star rating (15) plus optional text feedback for a mentor.
    - Update or delete your own review.
    - View all reviews and average rating for any mentor.
2. **Article Reviews**
    - Submit a star rating (15) plus optional text feedback for a mentor's article.
    - Update or delete your own article review.
    - View all reviews and average rating for any article.
3. **Admin Moderation**
    - List and remove any review (mentor or article) flagged as inappropriate.

## User Stories & Tasks

### Epic: Mentor Reviews CRUD

1. **Story REVIEW-MR-01** _Submit a Mentor Review_

    - **As** a logged-in mentee
    - **I want** to POST a new mentor review
    - **So that** I can rate and give feedback on my mentor
    - **Tasks:**
        - REVIEW-MR-01.1: Define DTO \CreateMentorReviewDto { MentorId, Rating, ReviewText }\.
        - REVIEW-MR-01.2: Implement \POST /api/reviews/mentors\ in \MentorReviewsController\.
        - REVIEW-MR-01.3: Add \IMentorReviewService.CreateAsync()\ and its implementation.

2. **Story REVIEW-MR-02** _Edit a Mentor Review_

    - **As** the original reviewer
    - **I want** to PUT an update to my mentor review
    - **So that** I can correct or refine my feedback
    - **Tasks:**
        - REVIEW-MR-02.1: Define DTO \UpdateMentorReviewDto { Id, Rating, ReviewText }\.
        - REVIEW-MR-02.2: Implement \PUT /api/reviews/mentors/{id}\.
        - REVIEW-MR-02.3: Add \IMentorReviewService.UpdateAsync()\.

3. **Story REVIEW-MR-03** _Delete a Mentor Review_

    - **As** the original reviewer
    - **I want** to DELETE my own mentor review
    - **So that** I can remove outdated or incorrect feedback
    - **Tasks:**
        - REVIEW-MR-03.1: Implement \DELETE /api/reviews/mentors/{id}\.
        - REVIEW-MR-03.2: Add \IMentorReviewService.DeleteAsync()\.
        - REVIEW-MR-03.3: Tests for proper soft/hard delete and authorization.

4. **Story REVIEW-MR-04** _List Mentor Reviews & Average_
    - **As** any user
    - **I want** to GET all reviews and average rating for a mentor
    - **So that** I can gauge mentor quality
    - **Tasks:**
        - REVIEW-MR-04.1: Define \GET /api/reviews/mentors/{mentorId}\ returning \ReviewDto\ list plus average.
        - REVIEW-MR-04.2: Implement aggregation in \MentorReviewService.GetByMentorAsync()\.
        - REVIEW-MR-04.3: Pagination support (e.g., \?page=&size=\).

---

### Epic: Article Reviews CRUD

**Key:** REVIEW-AR

1. **Story REVIEW-AR-01** _Submit an Article Review_

    - **As** any user
    - **I want** to rate & review a mentor's article
    - **Tasks:**
        - REVIEW-AR-01.1: Define \CreateArticleReviewDto { ArticleId, Rating, ReviewText }\.
        - REVIEW-AR-01.2: Implement \POST /api/reviews/articles\.
        - REVIEW-AR-01.3: \IArticleReviewService.CreateAsync()\.
        - REVIEW-AR-01.4: Unit & integration tests.

2. **Story REVIEW-AR-02** _Edit an Article Review_

    - **As** the reviewer
    - **I want** to update my article review
    - **Tasks:**
        - REVIEW-AR-02.1: Define \UpdateArticleReviewDto\.
        - REVIEW-AR-02.2: Implement \PUT /api/reviews/articles/{id}\.
        - REVIEW-AR-02.3: \IArticleReviewService.UpdateAsync()\.

3. **Story REVIEW-AR-03** _Delete an Article Review_

    - **As** the reviewer
    - **I want** to delete my article review
    - **Tasks:**
        - REVIEW-AR-03.1: Implement \DELETE /api/reviews/articles/{id}\.
        - REVIEW-AR-03.2: \IArticleReviewService.DeleteAsync()\.

4. **Story REVIEW-AR-04** _List Article Reviews & Average_
    - **As** any user
    - **I want** to view reviews and average for an article
    - **Tasks:**
        - REVIEW-AR-04.1: Implement \GET /api/reviews/articles/{articleId}\.
        - REVIEW-AR-04.2: Service aggregation and pagination.
        - REVIEW-AR-04.3: Tests for output correctness.

---

### Epic: Admin Moderation

**Key:** REVIEW-ADM

1. **Story REVIEW-ADM-01** _List All Reviews_

    - **As** an admin
    - **I want** to fetch all mentor & article reviews
    - **Tasks:**
        - REVIEW-ADM-01.1: Implement \GET /api/reviews/admin/mentors\ and \/admin/articles\.
        - REVIEW-ADM-01.2: Authorization policy \[Authorize(Roles=\"Admin,Moderator\")]\.
        - REVIEW-ADM-01.3: Tests to ensure only admins can access.

2. **Story REVIEW-ADM-02** _Delete Any Review_
    - **As** an admin
    - **I want** to remove any inappropriate review
    - **Tasks:**
        - REVIEW-ADM-02.1: Implement \DELETE /api/reviews/admin/mentors/{id}\ and \/articles/{id}\.
        - REVIEW-ADM-02.2: Service methods for forced delete.
        - REVIEW-ADM-02.3: Tests for admin-only access.

---

Each of these stories/tasks stays **fully within** the Reviews moduledefining its own schema, DbContext, entities, services, controllers, and testswhile only referencing external modules by ID.
