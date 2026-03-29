"use client";

import { MsalProvider as MsalReactProvider } from "@azure/msal-react";
import {
  PublicClientApplication,
  EventType,
  AuthenticationResult,
} from "@azure/msal-browser";
import { msalConfig } from "./msalConfig";
import { ReactNode, useMemo } from "react";

export default function MsalProvider({ children }: { children: ReactNode }) {
  const msalInstance = useMemo(() => {
    const instance = new PublicClientApplication(msalConfig);

    const accounts = instance.getAllAccounts();
    if (accounts.length > 0) {
      instance.setActiveAccount(accounts[0]);
    }

    instance.addEventCallback((event) => {
      if (
        event.eventType === EventType.LOGIN_SUCCESS &&
        event.payload
      ) {
        const result = event.payload as AuthenticationResult;
        instance.setActiveAccount(result.account);
      }
    });

    return instance;
  }, []);

  return (
    <MsalReactProvider instance={msalInstance}>{children}</MsalReactProvider>
  );
}
