# DUE_Fscharp

# Minimal Cut

## Minimal Cut (Greedy) Algorithm for Interval Scheduling

The minimal cut algorithm is a useful combinatorial algorithm for solving scheduling problems. The algorithm is very simple, yet it has a strong mathematical background in graph theory. Intervals are represented as nodes, and connections are established between them if they overlap.



### Mathematical Formulation

Let us consider a set of intervals:

![I set](https://latex.codecogs.com/png.image?%5Cmathcal%7BI%7D%3D%5C%7BI_1%2CI_2%2C%5Cdots%2CI_n%5C%7D)

where each interval is:

![interval](https://latex.codecogs.com/png.image?I_k%3D%5Ba_k%2Cb_k%5D%2C%20a_k%5Cleq%20b_k)



### Objective

Find a minimum cardinality set of points:

![P subset](https://latex.codecogs.com/png.image?P%5Csubseteq%5Cmathbb%7BR%7D)

such that every interval contains at least one point:

![condition](https://latex.codecogs.com/png.image?%5Cforall%20I_k%5Cin%5Cmathcal%7BI%7D%2C%20%5Cexists%20p%5Cin%20P%3A%20a_k%5Cleq%20p%5Cleq%20b_k)



### Greedy Algorithm

1. Sort intervals by right endpoint:

![sorting](https://latex.codecogs.com/png.image?b_1%5Cleq%20b_2%5Cleq%20%5Cdots%5Cleq%20b_n)

2. Initialize:

![empty set](https://latex.codecogs.com/png.image?P%3D%5Cemptyset)

3. For each interval:
   - If it is not yet covered:

![choose point](https://latex.codecogs.com/png.image?p%3Db_i)

   - Add to solution:

![add point](https://latex.codecogs.com/png.image?P%3DP%5Ccup%5C%7Bp%5C%7D)

   - Covered intervals satisfy:

![cover](https://latex.codecogs.com/png.image?a_j%5Cleq%20p%5Cleq%20b_j)



### Optimality

The greedy solution is optimal:

![optimality](https://latex.codecogs.com/png.image?%7CP_%7Bgreedy%7D%7C%3D%7CP_%7Boptimal%7D%7C)



### Complexity

![complexity](https://latex.codecogs.com/png.image?O(n%5Clog%20n))



### Remark

This problem is a special case of:

![set cover](https://latex.codecogs.com/png.image?Set%20Cover)

which is generally NP-hard, but for intervals it can be solved optimally with a greedy algorithm.


## Clique Cover in Interval Graphs

In graph theory, a **clique cover** is a set of cliques such that every vertex of the graph belongs to at least one clique.

For interval graphs, this problem has a special structure that allows an efficient solution.

---

### Graph Model

We define an interval graph as follows:

- Each interval corresponds to a node  
- Two nodes are connected if their intervals overlap  

Formally:

![edge](https://latex.codecogs.com/png.image?%5Ctext%7BEdge%20between%20%7D%20I_i%20%5Ctext%7B%20and%20%7D%20I_j%20%5Ciff%20%5BI_i%20%5Ccap%20I_j%20%5Cneq%20%5Cemptyset%5D)

---

## Objective

Find a minimum number of cliques such that every node is included in at least one clique.

![clique cover](https://latex.codecogs.com/png.image?%5Ctext%7BFind%20minimum%20%7D%20k%20%5Ctext%7B%20such%20that%20%7D%20V%20%3D%20C_1%20%5Ccup%20C_2%20%5Ccup%20%5Cdots%20%5Ccup%20C_k)

where each:

![clique](https://latex.codecogs.com/png.image?C_i%20%5Ctext%7B%20is%20a%20clique%7D)

---

## Greedy Strategy (Interval Partitioning)

For interval graphs, the clique cover problem is equivalent to **interval partitioning**:

- Each clique corresponds to a set of mutually overlapping intervals  
- Each clique can be represented by a vertical “cut” through overlapping intervals  

---

## Algorithm Idea

1. Sort intervals by starting point:

![sort](https://latex.codecogs.com/png.image?a_1%20%5Cleq%20a_2%20%5Cleq%20%5Cdots%20%5Cleq%20a_n)

2. Assign each interval to the first compatible group (clique)

3. If no group is available, create a new one

---

## Key Property

The minimum number of cliques equals the maximum number of overlapping intervals at any point:

![omega](https://latex.codecogs.com/png.image?%5Comega%28G%29%20%3D%20%5Cmax_x%20%7C%5C%7BI_k%20%3A%20x%20%5Cin%20I_k%5C%7D%7C)

---

## Result

![equality](https://latex.codecogs.com/png.image?%5Ctext%7BMinimum%20clique%20cover%7D%20%3D%20%5Comega%28G%29)

---

## Clique Cover in Interval Graphs

In graph theory, a **clique cover** is a set of cliques such that every vertex of the graph belongs to at least one clique.

For interval graphs, this problem has a special structure that allows an efficient solution.

---

## Graph Model

We define an interval graph as follows:

- Each interval corresponds to a node  
- Two nodes are connected if their intervals overlap  

Formally:

![edge](https://latex.codecogs.com/png.image?%5Ctext%7BEdge%20between%20%7D%20I_i%20%5Ctext%7B%20and%20%7D%20I_j%20%5Ciff%20%5BI_i%20%5Ccap%20I_j%20%5Cneq%20%5Cemptyset%5D)

---

## Objective

Find a minimum number of cliques such that every node is included in at least one clique.

![clique cover](https://latex.codecogs.com/png.image?%5Ctext%7BFind%20minimum%20%7D%20k%20%5Ctext%7B%20such%20that%20%7D%20V%20%3D%20C_1%20%5Ccup%20C_2%20%5Ccup%20%5Cdots%20%5Ccup%20C_k)

where each:

![clique](https://latex.codecogs.com/png.image?C_i%20%5Ctext%7B%20is%20a%20clique%7D)

---

## Greedy Strategy (Interval Partitioning)

For interval graphs, the clique cover problem is equivalent to **interval partitioning**:

- Each clique corresponds to a set of mutually overlapping intervals  
- Each clique can be represented by a vertical “cut” through overlapping intervals  

---

## Algorithm Idea

1. Sort intervals by starting point:

![sort](https://latex.codecogs.com/png.image?a_1%20%5Cleq%20a_2%20%5Cleq%20%5Cdots%20%5Cleq%20a_n)

2. Assign each interval to the first compatible group (clique)

3. If no group is available, create a new one

---

## Key Property

The minimum number of cliques equals the maximum number of overlapping intervals at any point:

![omega](https://latex.codecogs.com/png.image?%5Comega%28G%29%20%3D%20%5Cmax_x%20%7C%5C%7BI_k%20%3A%20x%20%5Cin%20I_k%5C%7D%7C)

---

## Result

![equality](https://latex.codecogs.com/png.image?%5Ctext%7BMinimum%20clique%20cover%7D%20%3D%20%5Comega%28G%29)

---

## Connection to Minimal Cut

- The **minimal cut (hitting set)** selects points that intersect all intervals  
- The **clique cover** groups intervals into overlapping sets  

These are dual perspectives of the same structure:

- one selects points  
- the other groups intervals  

---

## Complexity

![complexity](https://latex.codecogs.com/png.image?O(n%5Clog%20n))

due to sorting.

<img width="660" height="1000" alt="graph" src="https://github.com/user-attachments/assets/76a8265c-e41f-4056-ae04-495dd1700512" />
Resolutin will be the set P which has the interval end which covers the whole graph. (Red squares with interval endin the picture above)

