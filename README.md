# DUE_Fscharp

#Minimal cut
# Minimal Cut

## Minimal Cut (Greedy) Algorithm for Interval Scheduling

The minimal cut algorithm is a useful combinatorial algorithm for solving scheduling problems. The algorithm is very simple, yet it has a strong mathematical background in graph theory. Intervals are represented as nodes, and connections are established between them if they overlap.

---

## Mathematical Formulation

Let us consider a set of intervals:

![I set](https://latex.codecogs.com/png.image?%5Cmathcal%7BI%7D%3D%5C%7BI_1%2CI_2%2C%5Cdots%2CI_n%5C%7D)

where each interval is:

![interval](https://latex.codecogs.com/png.image?I_k%3D%5Ba_k%2Cb_k%5D%2C%20a_k%5Cleq%20b_k)

---

## Objective

Find a minimum cardinality set of points:

![P subset](https://latex.codecogs.com/png.image?P%5Csubseteq%5Cmathbb%7BR%7D)

such that every interval contains at least one point:

![condition](https://latex.codecogs.com/png.image?%5Cforall%20I_k%5Cin%5Cmathcal%7BI%7D%2C%20%5Cexists%20p%5Cin%20P%3A%20a_k%5Cleq%20p%5Cleq%20b_k)

---

## Greedy Algorithm

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

---

## Optimality

The greedy solution is optimal:

![optimality](https://latex.codecogs.com/png.image?%7CP_%7Bgreedy%7D%7C%3D%7CP_%7Boptimal%7D%7C)

---

## Complexity

![complexity](https://latex.codecogs.com/png.image?O(n%5Clog%20n))

---

## Remark

This problem is a special case of:

![set cover](https://latex.codecogs.com/png.image?Set%20Cover)

which is generally NP-hard, but for intervals it can be solved optimally with a greedy algorithm.
