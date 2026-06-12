import { HttpErrorResponse, HttpHeaderResponse } from "@angular/common/http";
import { inject, Injectable, signal, computed } from "@angular/core";

//JWT relaterade import
import { AuthService } from "../services/auth.service";
import { RegisterModel } from "../models/register.model";
import { LoginModel } from "../models/login.model";
import { AuthResponseModel } from "../models/auth-response.model";
