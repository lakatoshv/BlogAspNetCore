/**
 * JwtToken model.
 */
export class JwtToken {
  /**
   * JwtToken Access Token
   * @param AccessToken string
   * JwtToken Refresh Token
   * @param RefreshToken string
   * JwtToken Expires In
   * @param ExpiresIn number
   */
  constructor(
    public AccessToken: string,
    public RefreshToken: string,
    public ExpiresIn: number
  ) {}
}
