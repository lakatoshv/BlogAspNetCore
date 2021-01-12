import { User } from './User';

/**
 * Comment model.
 */
export class Comment {
  /**
   * Post Comment Id
   * @param id number
   * Post Comment PostId
   * @param postId number
   * Post Comment AuthorId
   * @param userId number
   * Post Comment User
   * @param user User
   * Post Comment Email
   * @param email string
   * Post Comment string
   * @param name string
   * Post Comment commentBody
   * @param commentBody string
   * Post Comment Created At
   * @param createdAt Date
   */
  constructor(
    public id: number = 0,
    public postId?: number,
    public userId?: string,
    public user?: User,
    public email?: string,
    public name?: string,
    public commentBody: string = '',
    public createdAt?: Date
  ) {}
}
