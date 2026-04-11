# DUE_Fscharp

# Minimal Cut

## 1.Minimal Cut (Greedy) Algorithm for Interval Scheduling

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


## 2.Clique Cover in Interval Graphs

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

### Objective

Find a minimum number of cliques such that every node is included in at least one clique.

![clique cover](https://latex.codecogs.com/png.image?%5Ctext%7BFind%20minimum%20%7D%20k%20%5Ctext%7B%20such%20that%20%7D%20V%20%3D%20C_1%20%5Ccup%20C_2%20%5Ccup%20%5Cdots%20%5Ccup%20C_k)

where each:

![clique](https://latex.codecogs.com/png.image?C_i%20%5Ctext%7B%20is%20a%20clique%7D)

---

### Greedy Strategy (Interval Partitioning)

For interval graphs, the clique cover problem is equivalent to **interval partitioning**:

- Each clique corresponds to a set of mutually overlapping intervals  
- Each clique can be represented by a vertical “cut” through overlapping intervals  

---

### Key Property

The minimum number of cliques equals the maximum number of overlapping intervals at any point:

![omega](https://latex.codecogs.com/png.image?%5Comega%28G%29%20%3D%20%5Cmax_x%20%7C%5C%7BI_k%20%3A%20x%20%5Cin%20I_k%5C%7D%7C)

---

### Result

![equality](https://latex.codecogs.com/png.image?%5Ctext%7BMinimum%20clique%20cover%7D%20%3D%20%5Comega%28G%29)

---

### Clique Cover in Interval Graphs

In graph theory, a **clique cover** is a set of cliques such that every vertex of the graph belongs to at least one clique.

For interval graphs, this problem has a special structure that allows an efficient solution.

---


due to sorting.

<img width="660" height="1000" alt="graph" src="https://github.com/user-attachments/assets/76a8265c-e41f-4056-ae04-495dd1700512" />
<p>Resolutin will be the set P which has the interval end which covers the whole graph. (Red squares with interval endin the picture above)

##3. Useful scenarios

## Use Cases of the Minimal Cut Algorithm

### 1. Task Scheduling and Checkpoints

In project management, tasks occupy time intervals.

- Each task is an interval [start, end]  
- A checkpoint is a specific time point  
- Goal: choose the minimum number of checkpoints so every task is covered  

The greedy algorithm selects optimal checkpoint times.

---

### 2. Surveillance Camera Placement

Consider a road or corridor divided into segments.

- Each segment is an interval  
- Cameras can be placed at specific positions  
- A camera covers all segments containing that point  

The algorithm finds the minimum number of cameras needed.

---

### 3. Sensor Placement

In monitoring systems:

- Each activity period is an interval  
- Sensors record at specific timestamps  
- Goal: minimize the number of measurements  

The algorithm ensures all intervals are observed efficiently.

---

### 4. Exam or Meeting Monitoring

In scheduling supervision:

- Each exam or meeting is an interval  
- Supervisors check at specific times  
- Goal: minimize the number of checks  

The algorithm determines optimal inspection times.

---

### 5. Maintenance Scheduling

For machines with service windows:

- Each maintenance window is an interval  
- A technician visits at specific times  
- Each visit can handle all active machines  

The algorithm minimizes the number of visits.

---

### 6. Network Packet Inspection

In networking:

- Each session is an interval  
- Inspection happens at discrete times  
- Goal: inspect all sessions with minimal checks  

The algorithm finds optimal inspection points.

---

### 7. Event Logging

In logging systems:

- Each event spans an interval  
- Logs are taken at specific times  
- Every event must be recorded at least once  

The algorithm minimizes logging operations.

---

### Summary

In all cases:

- Intervals represent time ranges or segments  
- Points represent decisions or actions  
- The goal is to cover all intervals with as few points as possible  

This is exactly what the minimal cut (greedy) algorithm solves optimally.

## 3. Getting Started
You can run with or without gui WPF by:
<p align="center"><b>dotnet run gui</b></p>

If you choose console application (without gui) you will get the following options:
```text
1 - Intervals (manual interval input)
2 - Greedy (random interval generation)
3 - Greedy and Brute Force simulation (comparison)
0 - Exit
```

## 4. Prerequisites
 - I create (populate) an externer program called Graphviz:
```text
dot - graphviz version 14.1.2 (20260124.0452)
```
Download: https://graphviz.org/download/

for windows 64.bit. After installation you have to add it to the PATH variable:
<img width="600" height="657" alt="image" src="https://github.com/user-attachments/assets/3b20f58a-b105-4223-b372-dc37419a7030" />

- .NET sdk 8.0
- Wisual Studio Code

## 5. Dependencies

- System
- System.Diagnostics
- System.Windows.Forms
- System.Drawing
- System.IO



