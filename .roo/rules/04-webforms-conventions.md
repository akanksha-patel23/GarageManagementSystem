## Web Forms specifics
- Turn off ViewState (EnableViewState="false") on controls that don't need it (e.g. static labels, read-only grids without in-place edit).
- Be deliberate with UpdatePanel + Timer + Wizard combos — test partial postback + validation behavior in isolation before wiring into a full page.
- Validators: pair every input with the correct validator type; CustomValidator logic (e.g. stock check) must call BAL, never query the DB directly from the validator.
- The job status board's Timer triggers a jQuery AJAX poll of GET /api/jobcards/{id}/status-history to refresh the Repeater — not a full UpdatePanel postback of the whole page.
- File upload - Vehicle photo / proof-of-completion FileUpload control: allow only .jpeg/.jpg/.png, maximum size 5 MB.
- GridView paging - Page size: 10 rows, for every GridView in the project.