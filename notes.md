# The orders supported by Trading Platform

## New Order                 ## Cancel Order

When we talk about trading, we need to talk about how the exchange modifies the order.

In some cases, If you send a modify order or buy new order, you will not be put at the back of the line but rather keep you where you at and keep you there.

### Implementation
We will cancel and move them back to the queue. 

Every Order will have a set of Orders that are used. 
* order ID
* username
* Secuirty ID

## You cannot have negative quantity but you might net negative prices.




