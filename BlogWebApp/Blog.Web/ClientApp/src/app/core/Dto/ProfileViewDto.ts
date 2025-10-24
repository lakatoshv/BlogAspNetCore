export class ProfileViewDto {
  /**
   * Profile view dto.
   * @param email string
   * @param firstName string | undefined
   * @param lastName string | undefined
   * @param phoneNumber string | undefined
   * @param password string | undefined
   * @param about string | undefined
   */
  constructor(
    public email: string,
    public firstName?: string | undefined,
    public lastName?: string | undefined,
    public phoneNumber?: string | undefined,
    public password?: string | undefined,
    public about?: string | undefined
  ) {}
}
