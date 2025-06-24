# 🌐 Your new SQL language.

Most of us needed to learn SQL and remember all the commands, some of which were easy to forget, as we didn't use them very often. So, if you create an MCP server that queries your existing Endpoint, which, for example, returns the list of customers, you can work with this much better than SQL. You will use simple English phrases to achieve your results, and it's even easier to format them nicely.

For example:

You can say something like this: 

### Could you please provide me with the list of all customers? 

### The MCP client (for example, I am using VS Code with GitHub Copilot). You switch your VS Code to ‘Agent mode’, then you type this:

The Client will interact with the MCP server created at the back end and will provide the data you requested. This data will be passed to the LLM, (GitHub Copilot), which will carry out the action you specify. You will see a response similar to this:


Here is the list of all customers:
1. Ana Machado (ID: 12a5dc0a-921f-4a80-b96c-1b2edb6eab7b), Birthdate: 1998-04-30, Adult: true
2. Jairo do Pandeiro (ID: 420706dd-bd4a-42df-8064-883e6c92b2eb), Birthdate: 1955-03-10, Adult: true
3. Paulo Machado (ID: 236d7261-bce4-4c1b-a8ca-e3b182793b53), Birthdate: 1955-05-21, Adult: true


### Could you please provide me with the list of customers in a well-organised format?


 Here is the list of customers in a nicely formatted table:

| Name             | ID                                    | Birthdate   | Adult |
|------------------|---------------------------------------|-------------|-------|
| Ana Machado      | 12a5dc0a-921f-4a80-b96c-1b2edb6eab7b  | 1998-04-30  | Yes   |
| Jairo do Pandeiro| 420706dd-bd4a-42df-8064-883e6c92b2eb  | 1955-03-10  | Yes   |
| Paulo Machado    | 236d7261-bce4-4c1b-a8ca-e3b182793b53  | 1955-05-21  | Yes   |



### Can you please add this new customer, Adrian Lima, 2008-11-28?


The new customer "Adrian Lima" with birthday "2008-11-28" has been added successfully. If you want to see the updated customer list, just let me know!

 
### Now, can you please give the list you gave before?


 Here is the updated list of customers in a nicely formatted table:

| Name             | ID                                    | Birthdate   | Adult |
|------------------|---------------------------------------|-------------|-------|
| Ana Machado      | 12a5dc0a-921f-4a80-b96c-1b2edb6eab7b  | 1998-04-30  | Yes   |
| Jairo do Pandeiro| 420706dd-bd4a-42df-8064-883e6c92b2eb  | 1955-03-10  | Yes   |
| Paulo Machado    | 236d7261-bce4-4c1b-a8ca-e3b182793b53  | 1955-05-21  | Yes   |
| Adrian Lima      | 80947393-563e-40f5-9725-45fa019103e0  | 2008-11-28  | No    |



### Can you please add a column with the ages?
 

Here is the updated list of customers with their ages (as of June 25, 2025):


| Name              | ID                                    | Birthdate   | Age | Adult |
|-------------------|---------------------------------------|-------------|-----|-------|
| Ana Machado       | 12a5dc0a-921f-4a80-b96c-1b2edb6eab7b  | 1998-04-30  | 27  | Yes   |
| Jairo do Pandeiro | 420706dd-bd4a-42df-8064-883e6c92b2eb  | 1955-03-10  | 70  | Yes   |
| Paulo Machado     | 236d7261-bce4-4c1b-a8ca-e3b182793b53  | 1955-05-21  | 70  | Yes   |
| Adrian Lima       | 80947393-563e-40f5-9725-45fa019103e0  | 2008-11-28  | 16  | No    |

Let me know if you need this in a different format or want to add more details!



### Could you please add a 'retired' column and flag all individuals over 65 years old?

Here is the updated list of customers with their ages and a "Retired" column (flagged "Yes" for those above 65 years old):

| Name              | ID                                    | Birthdate   | Age | Adult | Retired |
|-------------------|---------------------------------------|-------------|-----|-------|---------|
| Ana Machado       | 12a5dc0a-921f-4a80-b96c-1b2edb6eab7b  | 1998-04-30  | 27  | Yes   | No      |
| Jairo do Pandeiro | 420706dd-bd4a-42df-8064-883e6c92b2eb  | 1955-03-10  | 70  | Yes   | Yes     |
| Paulo Machado     | 236d7261-bce4-4c1b-a8ca-e3b182793b53  | 1955-05-21  | 70  | Yes   | Yes     |
| Adrian Lima       | 80947393-563e-40f5-9725-45fa019103e0  | 2008-11-28  | 16  | No    | No      |

Let me know if you need any more details or a different format!


 

## Is it the new SQL?






