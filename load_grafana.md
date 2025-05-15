# 🚀 Setting Up Grafana, Loki, and Prometheus with Docker

## Overview
This guide outlines how to set up **Grafana**, **Loki**, and **Prometheus** from scratch using Docker. These tools help with monitoring, logging, and visualization.

---

## 🔹 1️⃣ Pull Required Docker Images
First, download the necessary images:
```sh
docker pull grafana/grafana
docker pull grafana/loki
docker pull prom/prometheus
```

---

## 🔹 2️⃣ Create Configuration Files
If Prometheus requires custom configuration, create a basic `prometheus.yml`:
```sh
mkdir -p ~/grafana-stack/prometheus
nano ~/grafana-stack/prometheus/prometheus.yml
```
Example config:
```yaml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: "prometheus"
    static_configs:
      - targets: ["localhost:9090"]
```

---

## 🔹 3️⃣ Run Prometheus
Start Prometheus with volume mapping:
```sh
docker run -d --name=prometheus \
  -p 9090:9090 \
  -v ~/grafana-stack/prometheus/prometheus.yml:/etc/prometheus/prometheus.yml \
  prom/prometheus
```

---

## 🔹 4️⃣ Run Loki
Start Loki for logging:
```sh
docker run -d --name=loki \
  -p 3100:3100 \
  grafana/loki
```

---

## 🔹 5️⃣ Run Grafana
Start Grafana:
```sh
docker run -d --name=grafana \
  -p 3000:3000 \
  grafana/grafana
```

---

## 🔹 6️⃣ Verify Running Containers
Check that all services are running:
```sh
docker ps
```
Look for the **"Up"** status.

---

## 🔹 7️⃣ Access Services
Each service runs on specific ports:
- **Grafana:** [`http://localhost:3000`](http://localhost:3000)
- **Loki:** [`http://localhost:3100`](http://localhost:3100)
- **Prometheus:** [`http://localhost:9090`](http://localhost:9090)

_Default Grafana credentials:_
- **Username:** `admin`
- **Password:** `admin` _(change after login)_

---

## 🔹 8️⃣ Troubleshooting
If any container fails to start, check logs:
```sh
docker logs grafana
docker logs loki
docker logs prometheus
```
Restart if needed:
```sh
docker restart grafana loki prometheus
```

### 🔄 Automate Startup (Optional)
To ensure they restart automatically with Docker:
```sh
docker update --restart unless-stopped grafana loki prometheus
```

---

## 🎯 Conclusion
Now, Grafana, Loki, and Prometheus are ready to be used for monitoring and logging. Customize configurations as needed!

---


```

