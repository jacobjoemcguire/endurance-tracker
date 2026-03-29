"use client";

import AuthGuard from "../../components/AuthGuard";
import Link from "next/link";

export default function DashboardPage() {
  return (
    <AuthGuard>
      <main className="flex min-h-screen flex-col items-center justify-center p-8">
        <div className="max-w-2xl w-full space-y-6 text-center">
          <h1 className="text-3xl font-bold">Dashboard</h1>
          <p className="text-gray-500">
            Dashboard coming in Phase 3. Strava integration coming in Phase 2.
          </p>
          <Link
            href="/"
            className="inline-block text-blue-600 hover:text-blue-700 underline"
          >
            Back to home
          </Link>
        </div>
      </main>
    </AuthGuard>
  );
}
