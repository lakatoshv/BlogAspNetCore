/**
 * Error response.
 */
export class ErrorResponse{
    /**
     * @param error any
     */
    public error: any;
  
    /**
     * @param status any
     */
    public status: any
  
    /**
     * @param error any
     * @param status any
     */
    constructor(
      error: any,
      status: any) {
      this.error = error;
      this.status = status;
    }
  }
  