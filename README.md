# GildedRoseExpands

This project implements the beginnings of an API for a (fictional) small shopkeeper to begin selling her items online. It allows users to access information about the shop's inventory and buy items online.

## Usage:
### Getting the inventory
`GET api/inventory` will provide a list of currently available items.

`POST api/purchase/<itemId>` will request purchase of the given item. This API call must be called by an authenticated user. It will return a status code depending on the outcome of the purchase attempt as follows:  
0 - Item Successfully Purchased  
1 - Item Out of Stock  
2 - Payment Failed  
3 - Item Not Found  

Items have the following properties:  
`int ItemId`  
`string Name`  
`string Description`  
`decimal Price`  
`int Quantity`  

## Notes
### Data Format
I'm using JSON as my API's data format. It's more concise than XML, and the data I'm transmitting is simple enough that the structure of XML is unnecessary. I also chose to use an integer product ID to identify the item that the user wanted to purchase. This allows me to pass it as part of the URI, which is convenient for simplicity, but limiting because it requires requesting to purchase a single item at a time, rather than groups of items. I prefer using a product ID to a name, though, for three major reasons: IDs are guaranteed to be unique and names are not, IDs are easier to validate and less likely to have problematic characters in them, and using product IDs allows for changing the name (or A/B testing the product name) without the system thinking it's a totally new item.

*Note: I know that the example request/responses aren't complete. I'm not sure how to get a full raw HTTP request and response, and am short enough on time that I'm choosing to focus on finishing the API calls rather than getting perfect examples.*

#### Example inventory request/response:
Request:  
URI: https://localhost:44300/api/inventory  

Response:  
Status: 200 OK  
Body: [{"ItemId":1,"Name":"Fine Tunic","Description":"A sleeveless, knee-length garment.","Price":49.95,"Quantity":42},{"ItemId":2,"Name":"Fine Banana","Description":"A curved, yellow fruit.","Price":1.95,"Quantity":0}]

#### Example purchase request/response:
Request:  
URI: https://localhost:44300/api/purchase/1  
Authorization: Bearer jPCYFblZKfNSA3[...]  

Response:  
Status: 204 No Content

### Authentication
This API uses OAuth2 as its authentication mechanism. When creating a new WebAPI project, Visual Studio generated boilerplate OAuth2 code to provide authentication for individual accounts. As a professional developer, I believe that if I'm going to spend time changing a mechanism that already exists and functions, the burden of proof is on me to explain why there's a worthwhile ROI for that effort. Some basic testing of the authentication functionality and research about OAuth2 as an authentication mechanism convinced me that there wasn't a good argument for rewriting the service's authentication.

### Assumptions/Limitations
In order to limit the project's complexity, I have made the following assumptions about the type of inventory that the Gilded Rose is choosing to sell online:
* Items will be purchased one at a time (this is a pretty limiting assumption, and one that would be reasonably high on my list of things to address)
* There is a countable quantity of every item available (i.e., we are not selling e-books or other goods that have an effectively infinite supply)
* The shop's inventory is small enough to return all items when a client asks for the inventory, without causing performance problems for the client or server. When the shop grows big enough that this stops being true, I'd plan to add two things:
  * Ability for the client to specify filters for what to return (price, category, in-stock)
  * Paging - that is, the ability for the client to request N items, starting with item M*N
* All items should be displayed to API users, even if they're out of stock (this has the downside of meaning any discontinued items must be removed from the database entirely.)
* Any item sold can be purchased by any user, meaning:
  * No items have age restrictions for purchase
  * No items require a special permit to own
  * No products are illegal to ship to any of the regions we sell to
* None of the products are customizable, so the API doesn't accept information about desired color, individual requests, etc.

### Omitted functionality
There are some things that were out of scope to implement as part of this project, including:
* A payment system
* Address collection and verification
* The ability for the client to "reserve" items for a user to give them time to input payment and shipping details without others buying the item out from under them. If I was going to implement this, I would like to:
  * Add a "Reserve" API call in addition to the "Purchase" API call, that also requires authentication
  * Require the user to call the "Reserve" API call before calling "Purchase"
  * Store information about what item(s) a user has reserved and decrease the count of the product available when the product is reserved
  * Verify when "Purchase" is called that the item is reserved for that user before actually completing the purchase
  * Void/delete/remove the reservation after some amount of time (15 minutes?). Depending on how the other parts of the system worked, I'd do this one of two ways. One option is creating a service that ran every X minutes, deleting all of the reservations that were older than allowed. The other option is to create some sort of timer on the server that the "Reserve" call starts and when the timer expires, delete the reservation.
