# Recommendations module guidance

- This module owns interaction events, bookmarks/likes, recommendation results, ML.NET pipelines/models, and the `recommendations` PostgreSQL schema.
- Consume source-domain information only through Users, Materials, Ratings, and Scheduling Contracts; never query their DbContexts or reference implementation projects.
- Keep mentor and material pipelines separate where their inputs or scoring differ. Make feature weights, normalization, tie-breaking, top-N limits, and cold-start behavior explicit and deterministic.
- Avoid training or scoring work in request handlers when it can be a cancellable background pipeline. Stream/batch large datasets rather than materializing unbounded cross-products.
- Do not commit regenerated binary model artifacts unless the task explicitly requires them and their provenance/validation is documented.
- Test deterministic scoring components separately from ML training, and cover empty/sparse data, unknown IDs, duplicate interactions, ranking order, and authorization.
