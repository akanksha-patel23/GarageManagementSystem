Canonical Data Flows — follow exactly, do not improvise variations

This file is the single source of truth for request flows. If any other document or prior explanation conflicts with this file, this file wins.

A. Web-Forms-only Admin CRUD (Customers, Vehicles, ServiceCategories,

ServiceCatalog-admin, Parts, Technicians, Invoices)

[<Entity>List.aspx]  --PostBack-->  [<Entity>List.aspx.cs]
        --> [<Entity>BAL.cs] --> [<Entity>DAL.vb] --(ADO.NET SqlCommand)-->
        [sp<Action><Entity>] --> [Database]
No API involvement anywhere in this flow.
Applies identically to CustomerList.aspx, VehicleList.aspx, PartsList.aspx, TechnicianDirectory.aspx, InvoiceList.aspx/InvoiceGenerate.aspx, and the ServiceCategories/ServiceCatalog admin screens.
File naming: CustomerList.aspx (singular entity name), not CustomersList.aspx.
B. JobCard Creation (API-driven, the deliberate contrast case)
[JobCardWizard.aspx, final step] --(jQuery AJAX POST)--> [jobcard-wizard.js]
        --> POST /api/jobcards --> [JobCardsController.cs]
        --> [JobCardService.cs] --> [JobCardRepository.cs] --(Dapper, SqlTransaction)-->
        [spInsertJobCard + spInsertJobCardService + spInsertJobCardPart + spUpdatePartStock]
        --> [Database]
Entirely API-driven. Web Forms JobCardDAL/BAL never touch creation — only listing (spGetAllJobCards), viewing (spGetJobCardById), and status updates (spUpdateJobCardStatus, see flow E).
C. Vehicle Dropdown inside JobCard Wizard (NOT the VehicleList.aspx grid)
[JobCardWizard.aspx, vehicle step] --(client-side 'change' on Customer field)-->
        [jobcard-wizard.js] --(jQuery AJAX GET)--> GET /api/vehicles/{customerId}
        --> [VehiclesController.cs] --> [VehicleService.cs] --> [VehicleRepository.cs]
        --(Dapper)--> [spGetVehiclesByCustomer] --> [Database]
        <-- JSON --> JS populates the Vehicle DropDownList client-side
VehicleList.aspx (the admin grid) always uses Flow A. This flow is only for the wizard's dropdown.
D. Service Catalog Cascading Dropdown/CheckBoxList inside JobCard Wizard
[JobCardWizard.aspx, services step] --(client-side 'change' on Category dropdown,
        no postback)--> [jobcard-wizard.js] --(jQuery AJAX GET)-->
        GET /api/servicecatalog?categoryId=X --> [ServiceCatalogController.cs]
        --> [ServiceCatalogService.cs] --> [ServiceCatalogRepository.cs]
        --(Dapper)--> [spGetServicesByCategory] --> [Database]
        <-- JSON --> JS populates the CheckBoxList/ListBox client-side
No code-behind involvement — purely client-side jQuery AJAX.
This is NOT the same page as the ServiceCatalog admin CRUD screen, which follows Flow A instead. Do not conflate the two.
E. JobCard Status Board (Repeater, live-refreshing)
[JobCardStatusBoard.aspx: Repeater inside an UpdatePanel, driven by a Timer]
        --> Timer_Tick (server-side, JobCardStatusBoard.aspx.cs)
        --> [JobCardBAL.cs] --> [JobCardDAL.vb] --(ADO.NET)-->
        [spGetStatusHistoryByJobCard] --> [Database]
        --> Repeater.DataBind() server-side --> UpdatePanel partial postback
        refreshes only the Repeater region
Corrects earlier guidance: a server-side asp:Repeater cannot be rebound by pure client-side jQuery without a postback. UpdatePanel + Timer is the mechanism — genuinely exercises the Repeater control per the rubric.
GET /api/jobcards/{id}/status-history remains available on the API for any external/non-WebForms consumer, but the status board itself uses Flow A's BAL/DAL pattern for its live refresh, not the API.
jobcard-status.js is not required for this flow — remove it from the scaffold unless another consumer needs it.
F. Dashboard Chart (native ASP.NET Chart control, server-bound)
[Dashboard.aspx.cs, Page_Load] --(server-side HttpClient call)-->
        GET /api/dashboard/stats --> [DashboardController.cs]
        --> [DashboardService.cs] --> [DashboardRepository.cs]
        --(Dapper QueryMultiple)--> [spGetDashboardStats] --> [Database]
        <-- JSON --> Chart.Series bound server-side in code-behind
Corrects earlier guidance: System.Web.UI.DataVisualization.Charting is a server control and cannot be live-updated by client-side jQuery/Chart.js. The call to the API happens server-side via HttpClient, not via dashboard.js. Remove dashboard.js from the scaffold unless a manual "Refresh" button triggers a postback that re-runs Page_Load's fetch.
G. Job Status History (initial page load elsewhere, e.g. JobCard details view)
[<Any Page> showing status history, initial load] --> Code-behind Page_Load
        --> [JobCardBAL.cs] --> [JobCardDAL.vb] --> [spGetStatusHistoryByJobCard]
        --> [Database] --> bind to Repeater/GridView server-side
Same SP, reused for non-live-refresh contexts — no API call needed here either.