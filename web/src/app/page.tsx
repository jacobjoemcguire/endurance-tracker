"use client";

import { useIsAuthenticated, useMsal } from "@azure/msal-react";
import { InteractionStatus } from "@azure/msal-browser";
import { loginRequest, apiConfig } from "../lib/auth/msalConfig";
import { useEffect, useState } from "react";
import Link from "next/link";

export default function Home() {
  const isAuthenticated = useIsAuthenticated();
  const { instance, accounts, inProgress } = useMsal();
  const [healthStatus, setHealthStatus] = useState<string>("checking...");

  useEffect(() => {
    fetch(`${apiConfig.baseUrl}/api/health`)
      .then((res) => res.json())
      .then((data) => setHealthStatus(data.status))
      .catch(() => setHealthStatus("unavailable"));
  }, []);

  const handleLogin = () => {
    if (inProgress === InteractionStatus.None) {
      instance.loginRedirect(loginRequest);
    }
  };

  const handleLogout = () => {
    instance.logoutRedirect();
  };

  return (
    <main className="flex min-h-screen flex-col items-center justify-center p-8">
      <div className="max-w-md w-full space-y-8 text-center">
        <h1 className="text-4xl font-bold tracking-tight">EnduranceTracker</h1>
        <p className="text-gray-500">
          Training analysis and planning platform
        </p>

        <div className="mt-8 space-y-4">
          {isAuthenticated ? (
            <>
              <p className="text-lg">
                Welcome, <strong>{accounts[0]?.name || "Athlete"}</strong>
              </p>
              <Link
                href="/dashboard"
                className="inline-block rounded-lg bg-blue-600 px-6 py-3 text-white hover:bg-blue-700 transition"
              >
                Go to Dashboard
              </Link>
              <div>
                <button
                  onClick={handleLogout}
                  className="text-sm text-gray-500 hover:text-gray-700 underline"
                >
                  Sign out
                </button>
              </div>
            </>
          ) : (
            <button
              onClick={handleLogin}
              disabled={inProgress !== InteractionStatus.None}
              className="rounded-lg bg-blue-600 px-6 py-3 text-white hover:bg-blue-700 transition disabled:opacity-50"
            >
              Sign in with Microsoft
            </button>
          )}
        </div>

        <div className="mt-12 pt-8 border-t border-gray-200">
          <p className="text-xs text-gray-400">
            API Status:{" "}
            <span
              className={
                healthStatus === "healthy"
                  ? "text-green-500"
                  : "text-red-500"
              }
            >
              {healthStatus}
            </span>
          </p>
        </div>
      </div>
    </main>
  );
}
