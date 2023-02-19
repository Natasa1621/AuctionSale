# AuctionSale
A WPF MVVM application created according to ITAcademy assignment requirements with a database

The requirements:\
•	The application has a user management system implemented through a database. There must be at least two user statuses: an administrator and a regular user.\
•	The application retrieves auction data from the database.\
•	The main window of the application shows all the product that are currently offered. Each product data are displayed (the price, the last offer, the last bidder...).\
•	An unlogged user can view all the auction data, but does not have the option to bid.\
•	An ordinary logged-in user has the option of raising a bid.\
•	A logged-in administrator can add new products and delete existing ones. Each time a new product is added, its starting price, name and other information must be set.

The way the auction system functions:\
The auction for an added product starts at the moment of its addition, so its countdown begins. Each auction lasts two minutes. As long as users place their bids, the auction time starts from the zero and lasts additional two minutes. When a user places a bid for a product, the product price is increased by one euro. When the auction time expires, a user who placed the last bid is the winner, the auction is closed and ceased to be accessible to other users.

